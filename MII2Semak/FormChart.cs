using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MII2Semak
{
    public partial class FormChart : Form
    {
        readonly double[] Y;
        readonly List<double[]> XPos;
        readonly string name;
        readonly List<double> upSpeed;
        readonly List<double> downSpeed;
        bool isFirstDrawed;
        readonly List<string> names;
        readonly double absc;
        string finalAnswer;

        public FormChart(List<double[]> XPos, double[] Y, string name, double absc, List<string> names)
        {
            InitializeComponent();
            this.Y = Y;
            this.XPos = XPos;
            this.name = name;
            this.absc = absc;
            this.names = names;
            upSpeed = new List<double>();
            downSpeed = new List<double>();
            DrawChart();
        }

        private void DrawChart()
        {
            Chart myChart = new Chart();
            myChart.Parent = this;
            myChart.Dock = DockStyle.Fill;
            myChart.ChartAreas.Add(new ChartArea(name));
            Series mySeriesOfPoint = new Series();
            mySeriesOfPoint.ChartType = SeriesChartType.Line;
            mySeriesOfPoint.ChartArea = name;
            foreach (double[] x in XPos)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    mySeriesOfPoint.Points.AddXY(x[i], Y[i]);
                }
            }

            myChart.Series.Add(mySeriesOfPoint);

            label1.Text = GetAnswer();

            DrawIntersect(myChart);
        }

        private double[] revertY(double[] Y)
        {
            List<double> tmpList = new List<double>();
            foreach (double revertPos in Y)
            {
                if (revertPos == 0)
                {
                    tmpList.Add(1);
                }
                else
                {
                    tmpList.Add(0);
                }
            }
            return tmpList.ToArray();
        }

        private void DrawDopolnenie(Chart myChart, double[] x)
        {
            Series mySeriesOfPointRevert = new Series();
            mySeriesOfPointRevert.ChartType = SeriesChartType.Line;
            mySeriesOfPointRevert.ChartArea = name;
            mySeriesOfPointRevert.Color = Color.Red;

            for (int i = 0; i < x.Length; i++)
            {
                mySeriesOfPointRevert.Points.AddXY(x[i], revertY(Y)[i]);
            }

            myChart.Series.Add(mySeriesOfPointRevert);
        }

        private string GetAnswer()
        {
            double result = 0;
            bool isInsert = false;
            foreach (double[] x in XPos)
            {
                upSpeed.Add(1 / Math.Abs(x[1] - x[0]));
                downSpeed.Add(1 / Math.Abs(x[1] - x[2]));
            }


            for (int i = 0; i < XPos.Count; i++)
            {
                if (absc >= XPos[i][0] && absc <= XPos[i][1])
                {
                    isInsert = true;
                    double res = Math.Abs((double)((absc - XPos[i][0]) * upSpeed[i]));

                    if (res > result)
                    {
                        result = res;
                        finalAnswer = names[i];
                    }
                }

                if (absc > XPos[i][1] && absc <= XPos[i][2])
                {
                    isInsert = true;
                    double res = 1 - Math.Abs((double)(absc - XPos[i][1]) * downSpeed[i]);

                    if (res > result)
                    {
                        result = res;
                        finalAnswer = names[i];
                    }
                }
            }

            return finalAnswer;
        }

        private void DrawIntersect(Chart chart)
        {
            List<double?[]> Peresecheniya = new List<double?[]>();

            for(int i = 0; i < XPos.Count; i++)
            {
                double[] koeffMain = CalculateLineFunc(new double[] { XPos[i][1], 1 }, new double[] { XPos[i][2], 0 });

                for(int j = 0; j < XPos.Count; j++)
                {
                    if( j != i && XPos[i][1] < XPos[j][0])
                     q{
                        double[] koeffOther = CalculateLineFunc(new double[] { XPos[j][0], 0 }, new double[] { XPos[j][1], 1 });
                        var point = FindIntersection(koeffMain, koeffOther);
                        if (point != null)
                        {
                            var seria = new Series();
                            seria.ChartType = SeriesChartType.Line;
                            seria.ChartArea = name;
                            seria.Color = Color.Bisque;

                            seria.Points.AddXY(XPos[j][0], 0);
                            seria.Points.AddXY(point[0], point[1]);
                            seria.Points.AddXY(XPos[i][2], 0);

                            chart.Series.Add(seria);
                        }
                    }
                }
            }
        }
        
        private double[] CalculateLineFunc(double[] first, double[] second)
        {
            double A = first[1] - second[1];
            double B = second[0] - first[0];
            double C = first[0] * second[1] - second[0] * first[1];

            return new double[3] { A, B, -C };
        }

        private double?[] FindIntersection(double[] L1, double[] L2)
        {
            double D = L1[0] * L2[1] - L1[1] * L2[0];
            double Dx = L1[2] * L2[1] - L1[1] * L2[2];
            double Dy = L1[0] * L2[2] - L1[2] * L2[0];

            if (D != 0) {
                double x = Dx / D;
                double y = Dy / D;
                return new double?[] { x, y };
            }
            else
            {
                return null;
            }
        }
    }
}
