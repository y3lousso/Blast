using System;
using System.Collections.Generic;
using System.Linq;
using CenterSpace.NMath.Core;
using NAudio.Wave;

namespace BeatDetector
{
    public class SoundSignatureGenerator
    {
        public static List<List<bool>> GetSignature(string musicPath, float beat)
        {
            // Extract data and sample rate from audio file
            float sampleRate = GetMp3SampleRate(musicPath);
            float[] music = GetRawMp3Frames(musicPath);

            // Generate signature
            float[][] signature = CreateSignature(music, beat, sampleRate);

            int nbBands = (int) signature[1].Max() +1;
            int nbValues = signature[0].Length;
            int nbBeats = (int) (((float) music.Length) * beat / sampleRate / 60f)+1;

            float max = beat *signature[0].Max()/60f;

            List<List<bool>> boolSignature = new List<List<bool>>(nbBeats);
            for (int i = 0; i < nbBeats; i++)
            {
                 boolSignature.Add(Enumerable.Repeat(false, nbBands).ToList());
            }



            for (int i = 0; i < nbValues; i++)
            {
                int band = (int) signature[1][i];
                int time = (int) Math.Floor(signature[0][i]*beat/60f);
                boolSignature[time][band] = true;
            }

            return boolSignature;
        }


        /** Create the music signature : a list of 2D points (list[0] is abscissa, list[1] is ordinate)
         */
        private static float[][] CreateSignature(float[] signal, float beat, float sampleFrequency)
        {
            // Variables
            float fMin = 20f; // Hz
            float fMax = 2000f; // Hz
            int nbBands = 6;
            int n = signal.Length;
            float time = n / sampleFrequency;
            float barTime = 60f / beat;
            int nbBars = (int) (time / barTime);
            int nbSamplesPerBar = n / nbBars;
            float[] valuesT = new float[nbBars];
            for (int i = 0; i < nbBars; i++)
            {
                valuesT[i] = ((float) i * time) / ((float) (nbBars - 1));
            }

            // Gabor & audibles freq
            float stepF = sampleFrequency / (float) (nbSamplesPerBar);
            int nbValuesFS = (int) (fMax / stepF);
            float[] valuesFS = new float[nbValuesFS + 1];
            for (int i = 0; i < nbValuesFS + 1; i++)
            {
                valuesFS[i] = i * stepF;
            }

            FloatComplex[][] gabor = TGabor(signal, nbSamplesPerBar);

            // Sonogram (complex)
            FloatComplex[][] s = new FloatComplex[nbValuesFS][];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = new FloatComplex[nbBars];
                for (int j = 0; j < nbBars; j++)
                {
                    s[i][j] = gabor[i][j];
                }
            }

            // Partition
            float iMinPart = (float) Math.Log(fMin);
            float iMaxPart = (float) Math.Log(fMax);
            float stepPart = (iMaxPart - iMinPart) / (float) nbBands;
            float[] partition = new float[nbBands + 1];
            for (int i = 0; i < nbBands + 1; i++)
            {
                partition[i] = (float) Math.Exp(iMinPart + stepPart * ((float) i));
            }

            int[] indPartition = new int[nbBands + 1];
            for (int i = 0; i < nbBands; i++)
            {
                indPartition[i] = MinIndice(valuesFS, partition[i]);
            }

            indPartition[nbBands] = valuesFS.Length;

            // Compute sound signature
            float[][] signature = ES(s, indPartition, valuesT, valuesFS, nbBands);
            return signature;

        }

        /** Compute signature
         */
        private static float[][] ES(FloatComplex[][] s, int[] indPartition, float[] valuesT, float[] valuesFS, int nbBands)
        {
            float[][] signature = new float[2][];
            List<float> freqs = new List<float>();
            List<float> values = new List<float>();
            int nbBars = s[0].Length;

            for (int i = 0; i < nbBands; i++)
            {
                int iMin = indPartition[i];
                int iMax = Math.Min(indPartition[i + 1], s.Length);

                // Band extraction :
                FloatComplex[][] band = new FloatComplex[iMax - iMin][];
                for (int b1 = 0; b1 < iMax - iMin; b1++)
                {
                    band[b1] = new FloatComplex[nbBars];
                    for (int b2 = 0; b2 < nbBars; b2++)
                    {
                        band[b1][b2] = s[b1 + iMin][b2];
                    }
                }

                // Localisation of locales max
                float[][] localesMax = getLocalesMaximum(band);

                // threshold : mean + std
                float threshold = Mean(localesMax[1]) + STD(localesMax[1]);

                // Get frequencies upper than the threshold
                for (int j = 0; j < localesMax[0].Length; j++)
                {
                    if (localesMax[1][j] > threshold)
                    {
                        freqs.Add(valuesT[j]);
                        values.Add(i);
                    }
                }
            }

            signature[0] = freqs.ToArray();
            signature[1] = values.ToArray();
            return signature;
        }

        /** Gabor transform (complex values)
         */
        private static FloatComplex[][] TGabor(float[] signal, int nbSamplesPerBar)
        {
            int n = signal.Length;
            int nbBars = n / nbSamplesPerBar;

            FloatComplex[][] TG = new FloatComplex[nbSamplesPerBar][];
            var fftFoward = new FloatComplexForward1DFFT(nbSamplesPerBar);

            for (int i = 0; i < nbSamplesPerBar; i++)
            {
                TG[i] = new FloatComplex[nbBars];
            }

            for (int i = 0; i < nbBars; i++)
            {
                int iMin = i * nbSamplesPerBar;
                int iMax = (i + 1) * nbSamplesPerBar;

                FloatComplex[] sub = new FloatComplex[nbSamplesPerBar];
                for (int j = 0; j < iMax - iMin; j++)
                {
                    sub[j] = new FloatComplex(signal[j + iMin], 0);
                }

                fftFoward.FFTInPlace(sub);
                for (int j = 0; j < nbSamplesPerBar; j++)
                {
                    TG[j][i] = sub[j];
                }
            }

            return TG;
        }

        /*************************
         ******* FILES ***********
         *************************/

        /** Return the sample rate (Hz)
         */
        private static float GetMp3SampleRate(string filename)
        {
            float sampleRate;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {
                sampleRate = media.WaveFormat.SampleRate;
            }

            return sampleRate;
        }

        /** Return an array with all the music frames
         */
        private static float[] GetRawMp3Frames(string filename)
        {
            float[] floatBuffer;
            using (MediaFoundationReader media = new MediaFoundationReader(filename))
            {

                int byteBuffer32Length = (int) media.Length;
                int floatBufferLength = byteBuffer32Length / sizeof(float);

                IWaveProvider stream32 = new Wave16ToFloatProvider(media);
                WaveBuffer waveBuffer = new WaveBuffer(byteBuffer32Length);
                stream32.Read(waveBuffer, 0, byteBuffer32Length);
                floatBuffer = new float[floatBufferLength];

                for (int i = 0; i < floatBufferLength; i++)
                {
                    floatBuffer[i] = waveBuffer.FloatBuffer[i];
                }
            }

            return floatBuffer;
        }

        /*************************
         ******* Utils ***********
         *************************/

        /** Return the maximum of each column of the band
         * The first column of the return matrix contains the indices (int),
         * the second the values of the maximum
         */
        private static float[][] getLocalesMaximum(FloatComplex[][] band)
        {
            int n = band[0].Length;
            int sizeBand = band.Length;

            float[][] localesMax = new float[2][];
            localesMax[0] = new float[n];
            localesMax[1] = new float[n];

            for (int i = 0; i < n; i++)
            {
                int k = 0;
                int indMax = 0;
                float max = -1;
                while (k < sizeBand)
                {
                    if (Abs(band[k][i]) > max)
                    {
                        indMax = k;
                        max = Abs(band[k][i]);
                    }

                    k++;
                }

                localesMax[0][i] = indMax;
                localesMax[1][i] = max;
            }

            return localesMax;
        }

        /** Return the indice of the first number x in data such as x<value
         */
        private static int MinIndice(float[] data, float value)
        {
            int n = data.Length;
            int i = 0;
            int indMin = 0;
            bool isFound = false;

            while (i < n && !isFound)
            {
                if (data[i] > value)
                {
                    indMin = i;
                    isFound = true;
                }

                i++;
            }

            return indMin;
        }

        /** Mean
         */
        private static float Mean(float[] data)
        {
            return data.Sum() / data.Length;
        }

        /** Standard deviation 1D
         */
        private static float STD(float[] data)
        {
            double std = 0;
            if (data.Length > 0)
            {
                double avg = Mean(data);
                double sum = data.Sum(d => Math.Pow(d - avg, 2));
                std = Math.Sqrt((sum) / (data.Count() - 1));
            }

            return (float) std;

        }

        /** Complex abs : sqrt(x^2 + y^2)
         */
        private static float Abs(FloatComplex complex)
        {
            return (float) Math.Sqrt(Math.Pow(complex.Real, 2) + Math.Pow(complex.Imag, 2));
        }


    }
}