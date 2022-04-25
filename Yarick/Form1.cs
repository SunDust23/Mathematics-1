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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _coefsList = new List<double>();
            textBox1.KeyPress += Validation;
            textBox2.KeyPress += Validation;
            textBox3.KeyPress += Validation;
        }

        private List<double> _coefsList;

        private List<double> _xValue = new List<double>();
        private List<double> _yValue = new List<double>();
        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Нужно ввести данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double x = Convert.ToDouble(textBox1.Text);
            double y = Convert.ToDouble(textBox2.Text);

            _xValue.Add(x);
            _yValue.Add(y);

            dataGridView1.Rows.Add(x, y);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            _xValue.Clear();
            _yValue.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            listBox1.Items.Clear();
            _coefsList.Clear();
        }


        double[,] calculateDeltaMatrix()
        {
            int n = _xValue.Count;
            double[,] deltaMatrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                deltaMatrix[0, i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
            }

            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < n - i; j++)
                {
                    deltaMatrix[i, j] = deltaMatrix[i - 1, j + 1] - deltaMatrix[i - 1, j];
                }
            }
            return deltaMatrix;
        }

        double[] calculateCoefs(double[,] deltaMatrix)
        {
            int n = _xValue.Count;
            int factorial = 1;
            double h;
            double[] coef = new double[n];

            for (int i = 0; i < n; i++)
            {
                if (i != 0)
                {
                    factorial *= i;
                    h = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value) - Convert.ToDouble(dataGridView1.Rows[i - 1].Cells[0].Value);
                    coef[i] = deltaMatrix[i, 0] / (factorial * Math.Pow(h, i));
                }
                else
                {
                    coef[i] = Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value);
                }
            }


            return coef;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            listBox1.Items.Clear();
            _coefsList.Clear();

            int n = _xValue.Count;
            double[,] deltaMatrix = calculateDeltaMatrix();

            DataGridViewTextBoxColumn[] column = new DataGridViewTextBoxColumn[n];
            for (int i = 0; i < n; i++)
            {
                column[i] = new DataGridViewTextBoxColumn(); // выделяем память для объекта
                column[i].HeaderText = "Col" + i;
                column[i].Name = "Col" + i;
            }
            this.dataGridView2.Columns.AddRange(column);

            for (int i = 0; i < n; i++)
            {
                dataGridView2.Rows.Add();

                for (int j = 0; j < n - i; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = deltaMatrix[i, j];
                }
            }

            ///////////////////////////
            ///Подсчёт коэффициэнтов///
            ///////////////////////////

            double[] coef = calculateCoefs(deltaMatrix);

            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(coef[i]);
                _coefsList.Add(coef[i]);
            }
        }

        double calculateY(double[] coef, double x)
        {
            double y = 0;
            double xMult = 1;
            int n = _xValue.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    xMult *= x - Convert.ToDouble(dataGridView1.Rows[j].Cells[0].Value);
                }
                y += coef[i] * xMult;
                xMult = 1;
            }
            return y;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Нужно ввести данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double x = Convert.ToDouble(textBox3.Text);
            double[,] deltaMatrix = calculateDeltaMatrix();
            double[] coef = calculateCoefs(deltaMatrix);
            textBox4.Text = Convert.ToString(calculateY(coef, x));
        }

        private void графикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Chart(_coefsList, _xValue);
            form.Show();
        }

        private void примерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            _xValue.Clear();
            _yValue.Clear();

            _xValue.Add(0.02);
            _yValue.Add(0.0204);  
            
            _xValue.Add(0.04);
            _yValue.Add(0.0416);

            _xValue.Add(0.06);
            _yValue.Add(0.063599);

            _xValue.Add(0.08);
            _yValue.Add(0.086397);

            _xValue.Add(0.1);
            _yValue.Add(0.109992);

            _xValue.Add(0.12);
            _yValue.Add(0.134383);

            _xValue.Add(0.14);
            _yValue.Add(0.159568);

            _xValue.Add(0.16);
            _yValue.Add(0.185545);

            _xValue.Add(0.18);
            _yValue.Add(0.212313);

            _xValue.Add(0.2);
            _yValue.Add(0.239867);

            for (int i=0; i< _xValue.Count; i++)
            {
                dataGridView1.Rows.Add(_xValue[i], _yValue[i]);
            }
            
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new About();
            form.Show();
        }

        private void Validation(object sender, KeyPressEventArgs e)
        {
            var Sender = ((TextBox)sender);
            if (e.KeyChar == ',' && Sender.Text.Length > 0 && !Sender.Text.Contains(','))
                return;
            if (e.KeyChar == '-' && Sender.Text.Length == 0)
                return;
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != '\b')
                e.Handled = true;
        }
    }
}