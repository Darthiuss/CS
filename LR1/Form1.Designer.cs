namespace LR1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tab = new Button();
            label_xn = new Label();
            label_xk = new Label();
            label_xh = new Label();
            label_a = new Label();
            textBoxXN = new TextBox();
            dataGridView1 = new DataGridView();
            x = new DataGridViewTextBoxColumn();
            y = new DataGridViewTextBoxColumn();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            textBoxXK = new TextBox();
            textBoxA = new TextBox();
            textBoxXH = new TextBox();
            pictureBox1 = new PictureBox();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            роботаЗМасивамиToolStripMenuItem = new ToolStripMenuItem();
            одновимірніМасивиToolStripMenuItem = new ToolStripMenuItem();
            двовимірніМасивиToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tab
            // 
            tab.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tab.Location = new Point(395, 93);
            tab.Name = "tab";
            tab.Size = new Size(114, 33);
            tab.TabIndex = 0;
            tab.Text = "Розрахувати";
            tab.UseVisualStyleBackColor = true;
            tab.Click += tab_Click;
            // 
            // label_xn
            // 
            label_xn.AutoSize = true;
            label_xn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_xn.Location = new Point(40, 70);
            label_xn.Name = "label_xn";
            label_xn.Size = new Size(26, 21);
            label_xn.TabIndex = 1;
            label_xn.Text = "xn";
            // 
            // label_xk
            // 
            label_xk.AutoSize = true;
            label_xk.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_xk.Location = new Point(119, 70);
            label_xk.Name = "label_xk";
            label_xk.Size = new Size(25, 21);
            label_xk.TabIndex = 2;
            label_xk.Text = "xk";
            // 
            // label_xh
            // 
            label_xh.AutoSize = true;
            label_xh.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_xh.Location = new Point(195, 70);
            label_xh.Name = "label_xh";
            label_xh.Size = new Size(26, 21);
            label_xh.TabIndex = 3;
            label_xh.Text = "xh";
            // 
            // label_a
            // 
            label_a.AutoSize = true;
            label_a.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_a.Location = new Point(275, 70);
            label_a.Name = "label_a";
            label_a.Size = new Size(18, 21);
            label_a.TabIndex = 4;
            label_a.Text = "a";
            // 
            // textBoxXN
            // 
            textBoxXN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxXN.Location = new Point(23, 99);
            textBoxXN.Name = "textBoxXN";
            textBoxXN.Size = new Size(60, 29);
            textBoxXN.TabIndex = 5;
            textBoxXN.Text = "2.1";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { x, y });
            dataGridView1.Location = new Point(23, 151);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(342, 492);
            dataGridView1.TabIndex = 9;
            // 
            // x
            // 
            x.HeaderText = "x";
            x.Name = "x";
            // 
            // y
            // 
            y.HeaderText = "y";
            y.Name = "y";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Location = new Point(395, 151);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(598, 492);
            chart1.TabIndex = 10;
            chart1.Text = "chart1";
            // 
            // textBoxXK
            // 
            textBoxXK.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxXK.Location = new Point(100, 99);
            textBoxXK.Name = "textBoxXK";
            textBoxXK.Size = new Size(60, 29);
            textBoxXK.TabIndex = 11;
            textBoxXK.Text = "16.5";
            // 
            // textBoxA
            // 
            textBoxA.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxA.Location = new Point(253, 99);
            textBoxA.Name = "textBoxA";
            textBoxA.Size = new Size(60, 29);
            textBoxA.TabIndex = 12;
            textBoxA.Text = "10";
            // 
            // textBoxXH
            // 
            textBoxXH.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxXH.Location = new Point(177, 99);
            textBoxXH.Name = "textBoxXH";
            textBoxXH.Size = new Size(60, 29);
            textBoxXH.TabIndex = 13;
            textBoxXH.Text = "0.2";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(744, 37);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(249, 108);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, роботаЗМасивамиToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1019, 29);
            menuStrip1.TabIndex = 15;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(12, 25);
            // 
            // роботаЗМасивамиToolStripMenuItem
            // 
            роботаЗМасивамиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { одновимірніМасивиToolStripMenuItem, двовимірніМасивиToolStripMenuItem });
            роботаЗМасивамиToolStripMenuItem.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            роботаЗМасивамиToolStripMenuItem.Name = "роботаЗМасивамиToolStripMenuItem";
            роботаЗМасивамиToolStripMenuItem.Size = new Size(159, 25);
            роботаЗМасивамиToolStripMenuItem.Text = "Робота з масивами";
            // 
            // одновимірніМасивиToolStripMenuItem
            // 
            одновимірніМасивиToolStripMenuItem.Name = "одновимірніМасивиToolStripMenuItem";
            одновимірніМасивиToolStripMenuItem.Size = new Size(229, 26);
            одновимірніМасивиToolStripMenuItem.Text = "Одновимірні масиви";
            одновимірніМасивиToolStripMenuItem.Click += одновимірніМасивиToolStripMenuItem_Click;
            // 
            // двовимірніМасивиToolStripMenuItem
            // 
            двовимірніМасивиToolStripMenuItem.Name = "двовимірніМасивиToolStripMenuItem";
            двовимірніМасивиToolStripMenuItem.Size = new Size(229, 26);
            двовимірніМасивиToolStripMenuItem.Text = "Двовимірні масиви";
            двовимірніМасивиToolStripMenuItem.Click += двовимірніМасивиToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1019, 674);
            Controls.Add(pictureBox1);
            Controls.Add(textBoxXH);
            Controls.Add(textBoxA);
            Controls.Add(textBoxXK);
            Controls.Add(chart1);
            Controls.Add(dataGridView1);
            Controls.Add(textBoxXN);
            Controls.Add(label_a);
            Controls.Add(label_xh);
            Controls.Add(label_xk);
            Controls.Add(label_xn);
            Controls.Add(tab);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button tab;
        private Label label_xn;
        private Label label_xk;
        private Label label_xh;
        private Label label_a;
        private TextBox textBoxXN;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn x;
        private DataGridViewTextBoxColumn y;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private TextBox textBoxXK;
        private TextBox textBoxA;
        private TextBox textBoxXH;
        private PictureBox pictureBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem роботаЗМасивамиToolStripMenuItem;
        private ToolStripMenuItem одновимірніМасивиToolStripMenuItem;
        private ToolStripMenuItem двовимірніМасивиToolStripMenuItem;
    }
}
