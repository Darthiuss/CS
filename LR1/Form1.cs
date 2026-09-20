namespace LR1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tab_Click(object sender, EventArgs e)
        {
            Tabul tabul = new Tabul();
            double xn, xk, xh, a;
            xn = Convert.ToDouble(this.textBoxXN.Text.Replace('.', ','));
            xk = Convert.ToDouble(this.textBoxXK.Text.Replace('.', ','));
            xh = Convert.ToDouble(this.textBoxXH.Text.Replace('.', ','));
            a = Convert.ToDouble(this.textBoxA.Text.Replace('.', ','));

            dataGridView1.Rows.Clear();
            tabul.tab(xn, xk, xh, a);

            for (int i = 0; i < tabul.n; i++)
            {
                dataGridView1.Rows.Add(Math.Round(tabul.xy[i, 0], 2).ToString(),
                    Math.Round(tabul.xy[i, 1], 2).ToString());
                chart1.Series[0].Points.AddXY(tabul.xy[i, 0], tabul.xy[i, 1]);
            }
        }

        private void îäíîâèì³ðí³ÌàñèâèToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
            this.Hide();
        }

        private void äâîâèì³ðí³ÌàñèâèToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
            this.Hide();
        }
    }
}
