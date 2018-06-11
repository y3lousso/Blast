using System;
using System.Diagnostics;
using System.Linq;
using CenterSpace.NMath.Core;

namespace BeatDetector
{
    public class DWTBeatDetector
    {
        
        private int nbBands = 4;
        private float sampleRate = 44100; // Hz
        private float minBpm = 60;
        private float maxBpm = 220;


        public int Beat(float[] signal)
        {

            float bpm = -1;
            float mean = 0;
            int dcSumLenght = 0;
            float[] tempSignal = signal;
            float[] dcSum = new float[1];
            int maxDecimation = (int) Math.Pow(2, nbBands - 1);
            float[] approx;
            float[] details;
            int minIndex = (int) (60f * sampleRate / maxBpm) / maxDecimation;
            int maxIndex = (int) (60f * sampleRate / minBpm) / maxDecimation;


            for (int i = 0; i < nbBands; i++)
            {

                DWT(tempSignal, out approx, out details);
                dcSumLenght = details.Length / maxDecimation + 1;

                // Downsampling
                int pace = (int)Math.Pow(2, nbBands - i - 1);
                details = Undersample(details, pace);

                //FullWave rectification
                for (int j = 0; j < details.Length; j++)
                {
                    details[j] = Math.Abs(details[j]);
                }

                // Normalization
                mean = details.Sum() / details.Length;
                for (int j = 0; j < details.Length; j++)
                {
                    details[j] = details[j] - mean;
                }

                if (i == 0)
                {
                    dcSum = details;

                }
                else
                {
                    for (int j = 0; j < Math.Min(dcSumLenght, details.Length); j++)
                    {
                        dcSum[j] += details[j];
                    }
                }
                tempSignal = approx;

            }
            
            for (int j = 0; j < tempSignal.Length; j++)
            {
                tempSignal[j] = Math.Abs(tempSignal[j]);
            }

            mean = tempSignal.Sum() / tempSignal.Length;
            for (int j = 0; j < tempSignal.Length; j++)
            {
                tempSignal[j] = tempSignal[j] - mean;
            }

            for (int j = 0; j < Math.Min(dcSumLenght, tempSignal.Length); j++)
            {
                dcSum[j] = dcSum[j] + tempSignal[j];
            }
            

            // Autocorrelation
            float[] cor = Autocorrelate(dcSum);
            float[] cor2 = new float[maxIndex - minIndex];
            for (int i = 0; i < cor2.Length; i++)
            {
                cor2[i] = cor[i + minIndex];

            }

            int beat = DetectPeak(cor2);
            return (int) Math.Round(sampleRate * 60f / (beat + minIndex)) / maxDecimation;
        }


        private int DetectPeak(float[] signal)
        {
            int n = signal.Length;

            int indMax = 1;
            float max = signal.Min();
            for (int i = 1; i < n; i++)
            {
                if (Math.Abs(signal[i]) > max)
                {
                    indMax = i;
                    max = Math.Abs(signal[i]);
                }
            }

            int j = 0;

            // Locate index of max (positive max)
            while (j < n && indMax == -1)
            {
                if (Math.Abs(signal[j] - max) < 0.00001)
                {
                    indMax = j;
                }
                j++;
            }

            j = 0;
            // Locate index of max (negative max)
            while (j < n && indMax == -1)
            {
                if (Math.Abs(signal[j] + max) < 0.00001)
                {
                    indMax = j;

                }

                j++;
            }
            return indMax;
        }



        public void DWT(float[] signal, out float[] approxOutput, out float[] detailsOutput)
        {
            
            var data = new FloatVector(signal);
            var wavelet = new FloatWavelet(Wavelet.Wavelets.D4);
            var dwt = new FloatDWT(wavelet);

            dwt.DWT(data.DataBlock.Data, out approxOutput, out detailsOutput);
        }

        public float[] Undersample(float[] data, int pace)
        {
            int n = data.Length / pace;
            float[] output = new float[n];
            for (int i = 0; i < n; i++)
            {
                output[i] = data[i * pace];
            }

            return output;
        }

        public float[] Autocorrelate(float[] data)
        {
            int n = data.Length;
            float mean = data.Sum() / data.Length;
            float[] autocorrelate = new float[n];
            for (int k = 0; k < n; k++)
            {
                float s = 0;
                for (int j = 0; j < n; j++)
                {
                    if (k + j < n)
                    {
                        s += data[j] * data[k + j];
                    }
                }

                autocorrelate[k] = s;
            }

            return autocorrelate;
        }
    }
}