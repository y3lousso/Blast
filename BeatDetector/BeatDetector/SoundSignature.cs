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


        public void Main(float[] signal)
        {
            // Variables
            int n = signal.Length;
            int time = n / sampleFrequency;
            int nbBars = (int) (time / barTime);
            int nbSamplesPerBar = n / nbBars;
            float[] valuesT = new float[nbBars - 1];
            for (int i = 0; i < nbBars - 1; i++)
            {
                valuesT[i] = ((float) i * time) / ((float) (nbBars - 1));
            }

            // Gabor & audibles freq
            float stepF = sampleFrequency / (float) (nbSamplesPerBar);
            int nbValuesFS = (int) (fMax / stepF);
            float[] valuesFS = new float[nbValuesFS];
            for (int i = 0; i < nbValuesFS; i++)
            {
                valuesFS[i] = i * stepF;
            }

            FloatComplex[][] gabor = TGabor(signal, nbSamplesPerBar);

            // Sonogram (complex)
            FloatComplex[][] s = new FloatComplex[nbValuesFS][];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = new FloatComplex[nbSamplesPerBar];
                for (int j = 0; j < s.Length; j++)
                {
                    s[i][j] = gabor[i][j];
                }
            }

            // Partition
            float iMinPart = (int) Math.Log(fMin);
            float iMaxPart = (int) Math.Log(fMax);
            float stepPart = (iMaxPart - iMinPart) / (float) nbBands;
            float[] partition = new float[nbBands];
            for (int i = 0; i < nbBars; i++)
            {
                partition[i] = iMinPart + stepPart * ((float) i);
            }

            int[] indPartition = new int[nbBands + 1];
            for (int i = 0; i < nbBands; i++)
            {
                indPartition[i] = MinIndice(valuesFS, partition[i]);
            }

            indPartition[nbBands] = valuesFS.Length;

            // Compute sound signature
            float[][] signature = ES(s, indPartition, valuesT, valuesFS);

        }

        public float[][] ES(FloatComplex[][] s, int[] indPartition, float[] valuesT, float[] valuesFS)
        {
            float[][] signature = new float[2][];
            List<int> freqs = new List<int>();
            List<int> values = new List<int>();

            for (int i = 0; i < nbBands; i++)
            {
                int iMin = indPartition[i];
                int iMax = indPartition[i + 1];

                // Band extraction :
                float[][] band = new float[iMax-iMin][];
                NMar

            }
        }


        public FloatComplex[][] TGabor(float[] signal, int nbSamplesPerBar)
        {
            int n = signal.Length;
            int nbBars = n / nbSamplesPerBar;

            FloatComplex[][] TG = new FloatComplex[nbSamplesPerBar][];

            for (int i = 0; i < nbBars; i++)
            {
                int iMin = i * nbSamplesPerBar;
                int iMax = (i + 1) * nbSamplesPerBar - 1; 
                
                float[] sub = new float[iMax-iMin];
                for (int j = 0; j < iMax-iMin; j++)
                {
                    sub[j] = signal[j + iMin];
                }

                TG[i] = ComplexFFt(sub);
            }
            return TG;
        }
        
        
        public FloatComplex[] ComplexFFt(float[] signal)
        {

            int n = signal.Length;
            var fftVector = new FloatComplexVector();
            for (int i = 0; i < n; i++)
            {
                fftVector[i] = new FloatComplex(signal[i], 0);                
            }

            // Apply fft
            var fftFoward = new FloatComplexForward1DFFT(signal.Length);
            
            fftFoward.FFTInPlace(fftVector);

            return fftVector.ToArray();
        }

        public int MinIndice(float[] data, float value)
        {
            int n = data.Length;
            int i = 0;
            int indMin = 0;
            float min = data[0];
            bool isFound = false;

            while (i < n && !isFound)
            {
                if (data[i] < min)
                {
                    indMin = i;
                    min = data[i];
                    isFound = true;
                }

                i++;
            }

            return indMin;
        }

        public float Mean(float[] data)
        {
            return data.Sum() / data.Length;
        }

        public double STD(float[] data)
        {
            double std = 0;
            if (data.Length > 0)
            {
                double avg = Mean(data);
                double sum = data.Sum(d => Math.Pow(d - avg, 2));
                std = Math.Sqrt((sum) / (data.Count() - 1));
            }
            return std;

        }
    }
}