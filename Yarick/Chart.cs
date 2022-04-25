using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yarick
{
    public partial class Chart : Form
    {
        private List<double> _coefsList;
        private List<double> _xValue;
        public Chart(List<double> list, List<double> list2)
        {
            InitializeComponent();
            _coefsList = list;
            _xValue = list2;
        }

        private double f(double x)
        {
            double y = 0;
            double xMult = 1;
            int n = _coefsList.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    xMult *= x - _xValue[j];
                }
                y += _coefsList[i] * xMult;
                xMult = 1;
            }
            return y;
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            var minValue = _xValue.Min();
            var maxValue = _xValue.Max(); 
            //var minValue = Math.Floor(_xValue.Min());
            //var maxValue = Math.Ceiling(_xValue.Max());
            double h = Math.Round((maxValue - minValue) / _xValue.Count, 4);

            chart1.Series[0].ChartType =
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[0].BorderWidth = 5;
            chart1.Series[0].Color = Color.Yellow;

            chart1.ChartAreas[0].Axes[0].Title = "X";
            chart1.ChartAreas[0].Axes[1].Title = "Y";

            chart1.Series[0].Name = "Function";

            const int N = 10;

            for (double i = minValue-h; i <= maxValue+100*h; i+=h)
            {
                chart1.Series[0].Points.AddXY(i, f(i));
            }

            chart1.Series[1].ChartType =
            System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[1].BorderWidth = 5;
            chart1.Series[1].Color = Color.FromArgb(0,0,0,0);

            chart1.Series[1].Name = "Points";

            chart1.Series[1].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series[1].MarkerColor = Color.Purple;
            chart1.Series[1].MarkerSize = 9;

            for (int i = 0; i < _xValue.Count; i++)
            {
                chart1.Series[1].Points.AddXY(_xValue[i], f(_xValue[i]));
            }
        }
    }
}
