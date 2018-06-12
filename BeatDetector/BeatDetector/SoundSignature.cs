using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CenterSpace.NMath.Core;

namespace BeatDetector
{
    public class SoundSignature
    {

        private float fMin = 20f; // Hz
        private float fMax = 2000f; // Hz
        
        private int nbBands = 6;
        
        private float barTime = 0.2f; //seconds

        private int sampleFrequency = 44100; // Hz


        public float[][] Main(float[] signal)
        {
            // Variables
            int n = signal.Length;
            float time = n / sampleFrequency;
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
            float[] valuesFS = new float[nbValuesFS+1];
            for (int i = 0; i < nbValuesFS+1; i++)
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
            float[] partition = new float[nbBands+1];
            for (int i = 0; i < nbBands+1; i++)
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
            float[][] signature = ES(s, indPartition, valuesT, valuesFS);
            return signature;

        }

        public float[][] ES(FloatComplex[][] s, int[] indPartition, float[] valuesT, float[] valuesFS)
        {
            float[][] signature = new float[2][];
            List<float> freqs = new List<float>();
            List<float> values = new List<float>();
            int nbBars = s[0].Length;

            for (int i = 0; i < nbBands-1; i++)
            {
                int iMin = indPartition[i];
                int iMax = indPartition[i + 1];

                // Band extraction :
                FloatComplex[][] band = new FloatComplex[iMax-iMin][];
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
                        values.Add(valuesFS[(int) (localesMax[0][j]+ iMin)]);
                    }
                }
            }

            signature[0] = freqs.ToArray();
            signature[1] = values.ToArray();
            return signature;
        }

        /**
         * Return the maximum of each column of the band
         * The first column of the return matrix contains the indices (int),
         * the second the values of the maximum
         */
        public float[][] getLocalesMaximum(FloatComplex[][] band)
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



        /**
         * Gabor transform (complex values)
         */
        public FloatComplex[][] TGabor(float[] signal, int nbSamplesPerBar)
        {
            int n = signal.Length;
            int nbBars = n / nbSamplesPerBar;

            FloatComplex[][] TG = new FloatComplex[nbSamplesPerBar][];
            for (int i = 0; i < nbSamplesPerBar; i++)
            {
                TG[i] = new FloatComplex[nbBars];                
            }

            for (int i = 0; i < nbBars; i++)
            {
                int iMin = i * nbSamplesPerBar;
                int iMax = (i + 1) * nbSamplesPerBar; 
                
                float[] sub = new float[iMax-iMin];
                for (int j = 0; j < iMax-iMin; j++)
                {
                    sub[j] = signal[j + iMin];
                }

                FloatComplex[] subFFT = ComplexFFt(sub);
                for (int j = 0; j < nbSamplesPerBar; j++)
                {
                    TG[j][i] = subFFT[j];
                }
            }
            return TG;
        }
        
        
        /**
         * FFT inplace
         */
        public FloatComplex[] ComplexFFt(float[] signal)
        {

            int n = signal.Length;
            var fftVector = new FloatComplexVector(n);
            for (int i = 0; i < n; i++)
            {
                fftVector[i] = new FloatComplex(signal[i], 0);                
            }

            // Apply fft
            var fftFoward = new FloatComplexForward1DFFT(signal.Length);
            
            fftFoward.FFTInPlace(fftVector);

            return fftVector.ToArray();
        }

        /**
         * Return the indice of the first number x in data such as x<value
         */
        public int MinIndice(float[] data, float value)
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
        public float Mean(float[] data)
        {
            return data.Sum() / data.Length;
        }

        /** Standard deviation 1D
         */
        public float STD(float[] data)
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

        /**
         * Complex abs : sqrt(x^2 + y^2)
         */
        public float Abs(FloatComplex complex)
        {
            return (float) Math.Sqrt(Math.Pow(complex.Real, 2) + Math.Pow(complex.Imag, 2));
        }
    }
}