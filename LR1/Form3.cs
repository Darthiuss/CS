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
    public partial class Form3 : Form
    {
        public Form3()
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
            int n = 0, m = 0;
            n = Convert.ToInt32(textBoxN.Text);
            m = Convert.ToInt32(textBoxM.Text);
            Arrays2D twoDimArray = new Arrays2D(n, m);
            DataGridViewTextBoxColumn dvage;
            for (int i = 0; i < m; i++)
            {
                dvage = new DataGridViewTextBoxColumn();
                dvage.Width = 40;
                dataGridView1.Columns.Add(dvage);
            }
            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = twoDimArray.X_Length;
            dataGridView1.ColumnCount = twoDimArray.Y_Length;
            for (int i = 0; i < twoDimArray.X_Length; i++)
                for (int j = 0; j < twoDimArray.Y_Length; j++)
                    dataGridView1.Rows[i].Cells[j].Value = twoDimArray[i, j].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = 0, m = 0;
            n = Convert.ToInt32(textBoxN.Text);
            m = Convert.ToInt32(textBoxM.Text);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value) <= 5)
                        dataGridView1.Rows[i].Cells[j].Value = 111;
        }
    }
}
