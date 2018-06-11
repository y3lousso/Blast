using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace BeatDetector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetSize();
        }

        public void plotGraph(float[] signal)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Music";
            myPane.XAxis.Title.Text = "time";
            myPane.YAxis.Title.Text = "I";
            myPane.YAxis.Scale.Min = signal.Min();
            myPane.YAxis.Scale.Max = signal.Max();

            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = signal.Length -1;

            PointPairList list = new PointPairList();

            for (int i = 0; i < signal.Length; i++)
            {
                list.Add(i, signal[i]);
            }

            LineItem teamBCurve = myPane.AddCurve("Signal", list, Color.Blue, SymbolType.None);

            zedGraphControl1.AxisChange();

            SetSize();
        }

        private void SetSize()
        {
            zedGraphControl1.Location = new Point(0, 0);
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            SetSize();
        }

        private static float GetMax(float[] list)
        {
            float max = list[0];
            int indMax = 0;

            for (int i = 100; i < list.Length; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    indMax = i;
                }

            }
            return max;
        }

    }
}
