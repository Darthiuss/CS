namespace LR1
{
    partial class Form2
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
            A = new DataGridViewTextBoxColumn();
            label1 = new Label();
            textBoxN = new TextBox();
            button2 = new Button();
            labelARRAY = new Label();
            labelMEAN = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(88, 35);
            button1.TabIndex = 0;
            button1.Text = "Закрити";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { A });
            dataGridView1.Location = new Point(12, 78);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(221, 521);
            dataGridView1.TabIndex = 1;
            // 
            // A
            // 
            A.HeaderText = "A";
            A.Name = "A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(262, 78);
            label1.Name = "label1";
            label1.Size = new Size(37, 21);
            label1.TabIndex = 2;
            label1.Text = "N =";
            // 
            // textBoxN
            // 
            textBoxN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBoxN.Location = new Point(295, 75);
            textBoxN.Name = "textBoxN";
            textBoxN.Size = new Size(44, 29);
            textBoxN.TabIndex = 3;
            textBoxN.Text = "5";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.Location = new Point(544, 69);
            button2.Name = "button2";
            button2.Size = new Size(107, 35);
            button2.TabIndex = 4;
            button2.Text = "Обрахувати";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // labelARRAY
            // 
            labelARRAY.AutoSize = true;
            labelARRAY.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelARRAY.Location = new Point(265, 146);
            labelARRAY.Name = "labelARRAY";
            labelARRAY.Size = new Size(63, 21);
            labelARRAY.TabIndex = 5;
            labelARRAY.Text = "Масив: ";
            // 
            // labelMEAN
            // 
            labelMEAN.AutoSize = true;
            labelMEAN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            labelMEAN.Location = new Point(265, 179);
            labelMEAN.Name = "labelMEAN";
            labelMEAN.Size = new Size(77, 21);
            labelMEAN.TabIndex = 6;
            labelMEAN.Text = "Середнє: ";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(687, 616);
            Controls.Add(labelMEAN);
            Controls.Add(labelARRAY);
            Controls.Add(button2);
            Controls.Add(textBoxN);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn A;
        private Label label1;
        private TextBox textBoxN;
        private Button button2;
        private Label labelARRAY;
        private Label labelMEAN;
    }
}