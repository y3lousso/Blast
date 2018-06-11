using System;
using CenterSpace.NMath.Core;

namespace BeatDetector
{
    public class BPMTFF
    {
        private float acc = 1;
        private int npulses = 3;
        private float winlenght = 0.3f;
        private float minBpm = 60;
        private float maxBpm = 240;
        private static long[] bandLimits = {0, 2000, 4000, 8000, 16000, 32000};
        private int nbands = bandLimits.Length;
        private int maxfreq = 44100;


        public float[] CreateSampling(float[] signal)
        {
            int n = signal.Length;
            int sampleSize = 5 * 44100;

            float[] output = new float[sampleSize];
            for (int i = 0; i < sampleSize; i++)
            {
                output[i] = signal[sampleSize + i];

            }

            return output;
        }

        public float[] Extrapole(float[] signal, int prec)
        {
            int nbElements = signal.Length / prec;
            float[] output = new float[nbElements];

            for (int i = 0; i < nbElements; i++)
            {
                float e = 0.0f;
                for (int j = 0; j < prec; j++)
                {
                    e += signal[prec*i + j];
                }
                output[i] = e / prec;
            }
            return output;
        }



        public float[][] FilterBank(float[] signal)
        {
            long n = signal.Length;
            float[] dft = FFt(signal);
            int[] bl = new int[nbands];
            int[] br = new int[nbands];
            
            
            for (int i = 0; i < nbands-1; i++)
            {
                bl[i] = (int) (n * bandLimits[i] / maxfreq / 2);
                br[i] = (int) (n * bandLimits[i+1] / maxfreq / 2 - 1);
            }
            bl[nbands - 1] = (int) (n * bandLimits[nbands - 1] / maxfreq / 2);
            br[nbands - 1] = (int) (n / 2 -1);
            br[nbands - 1] = (int) (n / 2 -1);

            float[][] output = new float[nbands][];
            for (int i = 0; i < nbands; i++)
            {
                output[i] = new float[n];
            }


            for (int i = 0; i < nbands; i++)
            {
                for (int j = bl[i]; j < br[i]; j++)
                {
                    output[i][j] = dft[j];
                }
                for (int j = (int) (n-br[i]); j < n-bl[i]; j++)
                {
                    output[i][j] = dft[j];
                }
            }

            output[0][0] = 0;
            return output;

        }

        public float[][] Hwindow(float[][] fftsignal)
        {
            int n = fftsignal[0].Length;

            float[][] output = new float[nbands][];
            float[][] wave = new float[nbands][];
            for (int i = 0; i < nbands; i++)
            {
                output[i] = new float[n];
                wave[i] = new float[n];
            }

            float hannlen = winlenght * 2 * maxfreq;
            float[] hann = new float[n];
            for (int i = 0; i < hannlen; i++)
            {
                hann[i] = (float) Math.Pow(Math.Cos((i + 1) * (float) Math.PI / hannlen / 2), 2f);
            }

            for (int i = 0; i < nbands; i++)
            {
                wave[i] = Ifft(fftsignal[i]);
                for (int j = 0; j < n; j++)
                {
                    if (wave[i][j] < 0)
                    {
                        wave[i][j] = -wave[i][j];
                    }

                }
                wave[i] = FFt(wave[i]);
            }

            float[] ffthann = FFt(hann);
            for (int i = 0; i < nbands; i++)
            {
                float[] filtered = new float[n];
                for (int j = 0; j < n; j++)
                {
                    filtered[j] = wave[i][j] * ffthann[j];
                }

                output[i] = Ifft(filtered);
            }

            return output;
        }

        public float[][] DiffRect(float[][] fftSignal)
        {
            int n = fftSignal[0].Length;
            
            float[][] output = new float[nbands][];
            for (int i = 0; i < nbands; i++)
            {
                output[i] = new float[n];
            }

            for (int i = 0; i < nbands; i++)
            {
                for (int j = 4; j < n; j++)
                {
                    float d = fftSignal[i][j] - fftSignal[i][j - 1];
                    if (d > 0)
                    {
                        output[i][j] = d;
                    }
                }
            }
            return output;
        }

        public float[] TimeComb(float[][] fftSignal)
        {
            float[] tempMOVE = new float[(int) (maxBpm/acc) +1];

            float bpm = minBpm;
            float sbpm = 0;
            float maxe = 0;

            int n = fftSignal[0].Length;
            
            float[][] dft = new float[nbands][];
            for (int i = 0; i < nbands; i++)
            {
                dft[i] = FFt(fftSignal[i]);
            }

            while (bpm <= maxBpm)
            {
                float e = 0;

                float[] fil = new float[n];
                int nstep = (int) (120 * maxfreq / bpm);

                for (int a = 0; a < npulses; a++)
                {
                    fil[a * nstep] = 1;
                }

                float[] dftfil = FFt(fil);

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        e += (float) Math.Pow(Math.Abs(dftfil[j] * dft[i][j]), 2);
                    }
                }

                if (e > maxe)
                {
                    sbpm = bpm;
                    maxe = e;
                }

                tempMOVE[(int) bpm] = e;
                bpm += acc;
                Console.WriteLine(bpm);
            }
            return tempMOVE;

        }


        public float[] FFt(float[] signal)
        {
            
            var fftVector = new FloatVector(signal);

            // Apply fft
            var fftFoward = new FloatForward1DFFT(signal.Length);
            fftFoward.FFTInPlace(fftVector);

            return fftVector.ToArray();
        }
        
        public float[] Ifft(float[] signal)
        {

            var ifftVector = new FloatVector(signal);

            // Apply ifft
            var fftBackward = new FloatSymmetricBackward1DFFT(signal.Length);
            fftBackward.SetScaleFactorByLength();
            fftBackward.FFTInPlace(ifftVector);

            return ifftVector.ToArray();
        }
    }
}