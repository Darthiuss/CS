using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            labelARRAY.Text = "Масив: ";
            labelMEAN.Text = "Середнє: ";
            int N = Convert.ToInt32(this.textBoxN.Text.Replace('.', ','));
            int[] mas = new int[N];
            for (int i = 0; i < N; i++)
            {
                mas[i] = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
            }
            Arrays array = new Arrays(mas);
            for(int i = 0; i < N; i++)
            {
                if(i == N-1)
                {
                    labelARRAY.Text += array[i].ToString();
                    break;
                }
                labelARRAY.Text += array[i].ToString();
                labelARRAY.Text += ", ";
            }
            labelMEAN.Text += array.mean().ToString();
        }
    }
}
