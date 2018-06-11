using System;
using CenterSpace.NMath.Core;

namespace BeatDetector
{
    public class SoundSignature
    {

        private float fMin = 20f; // Hz
        private float fMax = 2000f; // Hz
        
        private int nbBands = 6;
        
        private float barDelay = 0.2f; //seconds

        private int sampleFrequency = 44200; // Hz


        public float[][] TGabor(float[] signal, int nbSamplesPerBar)
        {
            int n = signal.Length;
            int nbBars = n / nbSamplesPerBar;

            float[][] TG = new float[nbSamplesPerBar][];
            for (int i = 0; i < nbSamplesPerBar; i++)
            {
                TG[i] = new float[nbBars];
            }

            for (int i = 0; i < nbBars; i++)
            {
                int iMin = i * nbSamplesPerBar + 1;
                int iMax = Math.Min(i * nbSamplesPerBar, n);
                
                float[] sub = new float[iMax-iMin];
                for (int j = 0; j < iMax-iMin; j++)
                {
                    sub[j] = signal[j + iMin];
                }

                float[] fft = FFt(sub);
            }
            return null;
        }
        
        
        public float[] FFt(float[] signal)
        {
            
            var fftVector = new FloatVector(signal);

            // Apply fft
            var fftFoward = new FloatForward1DFFT(signal.Length);
            
            fftFoward.FFTInPlace(fftVector);

            return fftVector.ToArray();
        }
    }
}