namespace LR1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            dataGridView1 = new DataGridView();
            label1 = new Label();
            textBoxN = new TextBox();
            label2 = new Label();
            textBoxM = new TextBox();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(89, 35);
            button1.TabIndex = 0;
            button1.Text = "Закрити";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 67);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(405, 331);
            dataGridView1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(443, 67);
            label1.Name = "label1";
            label1.Size = new Size(37, 21);
            label1.TabIndex = 3;
            label1.Text = "N =";
            // 
            // textBoxN
            // 
            textBoxN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxN.Location = new Point(477, 64);
            textBoxN.Name = "textBoxN";
            textBoxN.Size = new Size(44, 29);
            textBoxN.TabIndex = 4;
            textBoxN.Text = "5";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(443, 108);
            label2.Name = "label2";
            label2.Size = new Size(39, 21);
            label2.TabIndex = 5;
            label2.Text = "M =";
            // 
            // textBoxM
            // 
            textBoxM.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxM.Location = new Point(477, 105);
            textBoxM.Name = "textBoxM";
            textBoxM.Size = new Size(44, 29);
            textBoxM.TabIndex = 6;
            textBoxM.Text = "4";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.Location = new Point(443, 140);
            button2.Name = "button2";
            button2.Size = new Size(87, 35);
            button2.TabIndex = 7;
            button2.Text = "Вивести";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button3.Location = new Point(443, 181);
            button3.Name = "button3";
            button3.Size = new Size(87, 34);
            button3.TabIndex = 8;
            button3.Text = "Змінити";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 417);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBoxM);
            Controls.Add(label2);
            Controls.Add(textBoxN);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Name = "Form3";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dataGridView1;
        private Label label1;
        private TextBox textBoxN;
        private Label label2;
        private TextBox textBoxM;
        private Button button2;
        private Button button3;
    }
}