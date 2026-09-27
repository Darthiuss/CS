namespace LR2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCulc_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(textBoxA.Text);
                int b = Convert.ToInt32(textBoxB.Text);
                int c = Convert.ToInt32(textBoxC.Text);

                var task1 = new Task1(a, b, c);
                labelRes.Text = "Результат: " + task1.CalculateSumOfCubes().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCulc2_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(textBoxA2.Text);
                int b = Convert.ToInt32(textBoxB2.Text);

                var task2 = new Task2(a, b);
                labelRes2.Text = "Результат: " + task2.CalculateSum().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCulc3_Click(object sender, EventArgs e)
        {
            try
            {
                double a = Convert.ToDouble(textBoxA3.Text);
                double b = Convert.ToDouble(textBoxB3.Text);
                double c = Convert.ToDouble(textBoxC3.Text);

                var triangle = new Triangle(a, b, c);
                double area = triangle.CalculateArea();
                string type = triangle.GetTriangleType();

                labelRes3.Text = $"Площа: {area:F4}\nТип: {type}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form mainForm = Application.OpenForms[0];
            mainForm.Show();
            this.Close();
        }
    }
}
