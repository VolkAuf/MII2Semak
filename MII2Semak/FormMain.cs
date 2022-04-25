using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MII2Semak
{
    public partial class FormMain : Form
    {
        List<double[]> X = new List<double[]>();
        List<string> Names = new List<string>();
        double[] Y = new double[] { 0, 1, 0 };

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            X.Add(new double[] { Double.Parse(textBox1.Text), Double.Parse(textBox2.Text), Double.Parse(textBox3.Text) });
            Names.Add(textBox4.Text);
            var chart = new FormChart(X, Y, textBox4.Text, textBox5.Text.Equals(string.Empty) ? 0 : double.Parse(textBox5.Text), Names);
            chart.Show();
        }
    }
}
