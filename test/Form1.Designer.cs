namespace test
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.settingTabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.digitsNum = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.OutputTypePanel = new System.Windows.Forms.Panel();
            this.Decimal = new System.Windows.Forms.RadioButton();
            this.Binary = new System.Windows.Forms.RadioButton();
            this.ActivationFunctionPanel = new System.Windows.Forms.Panel();
            this.BipolarSigmoid = new System.Windows.Forms.RadioButton();
            this.Sigmoid = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.saveNetBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.resetNetBtn = new System.Windows.Forms.Button();
            this.netXTB = new System.Windows.Forms.TextBox();
            this.netYTB = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.kLearnTB = new System.Windows.Forms.TextBox();
            this.LearnAllBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.stopLearnTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.alphaTextBox = new System.Windows.Forms.TextBox();
            this.sLearnTextBox = new System.Windows.Forms.TextBox();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.captchaResultLabel = new System.Windows.Forms.Label();
            this.GenerateBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.heightRange = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.lineCount = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.settingTabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitsNum)).BeginInit();
            this.OutputTypePanel.SuspendLayout();
            this.ActivationFunctionPanel.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineCount)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog2";
            this.openFileDialog3.FileOk += new System.ComponentModel.CancelEventHandler(this.openNetFileDialog_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveNerFileDialog_FileOk);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lineCount);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.heightRange);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.digitsNum);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.settingTabControl);
            this.tabPage1.Controls.Add(this.resultTextBox);
            this.tabPage1.Controls.Add(this.captchaResultLabel);
            this.tabPage1.Controls.Add(this.GenerateBtn);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(987, 707);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // settingTabControl
            // 
            this.settingTabControl.Controls.Add(this.tabPage2);
            this.settingTabControl.Controls.Add(this.tabPage3);
            this.settingTabControl.Location = new System.Drawing.Point(544, 6);
            this.settingTabControl.Name = "settingTabControl";
            this.settingTabControl.SelectedIndex = 0;
            this.settingTabControl.Size = new System.Drawing.Size(437, 183);
            this.settingTabControl.TabIndex = 60;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.OutputTypePanel);
            this.tabPage2.Controls.Add(this.ActivationFunctionPanel);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.saveNetBtn);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.resetNetBtn);
            this.tabPage2.Controls.Add(this.netXTB);
            this.tabPage2.Controls.Add(this.netYTB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(429, 157);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Сеть";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // digitsNum
            // 
            this.digitsNum.Location = new System.Drawing.Point(377, 67);
            this.digitsNum.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.digitsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.digitsNum.Name = "digitsNum";
            this.digitsNum.Size = new System.Drawing.Size(48, 20);
            this.digitsNum.TabIndex = 66;
            this.digitsNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.digitsNum.ValueChanged += new System.EventHandler(this.digitsNum_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(431, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 65;
            this.label8.Text = "Длина";
            // 
            // OutputTypePanel
            // 
            this.OutputTypePanel.Controls.Add(this.Decimal);
            this.OutputTypePanel.Controls.Add(this.Binary);
            this.OutputTypePanel.Location = new System.Drawing.Point(6, 93);
            this.OutputTypePanel.Name = "OutputTypePanel";
            this.OutputTypePanel.Size = new System.Drawing.Size(99, 52);
            this.OutputTypePanel.TabIndex = 63;
            // 
            // Decimal
            // 
            this.Decimal.AutoSize = true;
            this.Decimal.Location = new System.Drawing.Point(3, 28);
            this.Decimal.Name = "Decimal";
            this.Decimal.Size = new System.Drawing.Size(63, 17);
            this.Decimal.TabIndex = 61;
            this.Decimal.Text = "Decimal";
            this.Decimal.UseVisualStyleBackColor = true;
            // 
            // Binary
            // 
            this.Binary.AutoSize = true;
            this.Binary.Checked = true;
            this.Binary.Location = new System.Drawing.Point(3, 4);
            this.Binary.Name = "Binary";
            this.Binary.Size = new System.Drawing.Size(54, 17);
            this.Binary.TabIndex = 60;
            this.Binary.TabStop = true;
            this.Binary.Text = "Binary";
            this.Binary.UseVisualStyleBackColor = true;
            // 
            // ActivationFunctionPanel
            // 
            this.ActivationFunctionPanel.Controls.Add(this.BipolarSigmoid);
            this.ActivationFunctionPanel.Controls.Add(this.Sigmoid);
            this.ActivationFunctionPanel.Location = new System.Drawing.Point(6, 35);
            this.ActivationFunctionPanel.Name = "ActivationFunctionPanel";
            this.ActivationFunctionPanel.Size = new System.Drawing.Size(99, 52);
            this.ActivationFunctionPanel.TabIndex = 62;
            // 
            // BipolarSigmoid
            // 
            this.BipolarSigmoid.AutoSize = true;
            this.BipolarSigmoid.Checked = true;
            this.BipolarSigmoid.Location = new System.Drawing.Point(3, 28);
            this.BipolarSigmoid.Name = "BipolarSigmoid";
            this.BipolarSigmoid.Size = new System.Drawing.Size(94, 17);
            this.BipolarSigmoid.TabIndex = 61;
            this.BipolarSigmoid.TabStop = true;
            this.BipolarSigmoid.Text = "BipolarSigmoid";
            this.BipolarSigmoid.UseVisualStyleBackColor = true;
            // 
            // Sigmoid
            // 
            this.Sigmoid.AutoSize = true;
            this.Sigmoid.Location = new System.Drawing.Point(3, 4);
            this.Sigmoid.Name = "Sigmoid";
            this.Sigmoid.Size = new System.Drawing.Size(62, 17);
            this.Sigmoid.TabIndex = 60;
            this.Sigmoid.Text = "Sigmoid";
            this.Sigmoid.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(348, 35);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 43;
            this.button4.Text = "load";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.loadNetBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(246, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(12, 13);
            this.label7.TabIndex = 59;
            this.label7.Text = "x";
            // 
            // saveNetBtn
            // 
            this.saveNetBtn.Location = new System.Drawing.Point(348, 6);
            this.saveNetBtn.Name = "saveNetBtn";
            this.saveNetBtn.Size = new System.Drawing.Size(75, 23);
            this.saveNetBtn.TabIndex = 44;
            this.saveNetBtn.Text = "save";
            this.saveNetBtn.UseVisualStyleBackColor = true;
            this.saveNetBtn.Click += new System.EventHandler(this.saveNetBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(328, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 58;
            this.label6.Text = "y";
            // 
            // resetNetBtn
            // 
            this.resetNetBtn.Location = new System.Drawing.Point(348, 65);
            this.resetNetBtn.Name = "resetNetBtn";
            this.resetNetBtn.Size = new System.Drawing.Size(75, 23);
            this.resetNetBtn.TabIndex = 55;
            this.resetNetBtn.Text = "reset";
            this.resetNetBtn.UseVisualStyleBackColor = true;
            this.resetNetBtn.Click += new System.EventHandler(this.resetNetBtn_Click);
            // 
            // netXTB
            // 
            this.netXTB.Location = new System.Drawing.Point(192, 67);
            this.netXTB.Name = "netXTB";
            this.netXTB.Size = new System.Drawing.Size(48, 20);
            this.netXTB.TabIndex = 57;
            this.netXTB.TextChanged += new System.EventHandler(this.netXTB_TextChanged);
            // 
            // netYTB
            // 
            this.netYTB.Location = new System.Drawing.Point(274, 67);
            this.netYTB.Name = "netYTB";
            this.netYTB.Size = new System.Drawing.Size(48, 20);
            this.netYTB.TabIndex = 56;
            this.netYTB.TextChanged += new System.EventHandler(this.netYTB_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.kLearnTB);
            this.tabPage3.Controls.Add(this.LearnAllBtn);
            this.tabPage3.Controls.Add(this.stopBtn);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.stopLearnTextBox);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.alphaTextBox);
            this.tabPage3.Controls.Add(this.sLearnTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(429, 157);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Обучение";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // kLearnTB
            // 
            this.kLearnTB.Location = new System.Drawing.Point(88, 7);
            this.kLearnTB.Name = "kLearnTB";
            this.kLearnTB.Size = new System.Drawing.Size(48, 20);
            this.kLearnTB.TabIndex = 55;
            this.kLearnTB.TextChanged += new System.EventHandler(this.kLearnTB_TextChanged);
            // 
            // LearnAllBtn
            // 
            this.LearnAllBtn.Location = new System.Drawing.Point(6, 6);
            this.LearnAllBtn.Name = "LearnAllBtn";
            this.LearnAllBtn.Size = new System.Drawing.Size(75, 23);
            this.LearnAllBtn.TabIndex = 6;
            this.LearnAllBtn.Text = "learnAll";
            this.LearnAllBtn.UseVisualStyleBackColor = true;
            this.LearnAllBtn.Click += new System.EventHandler(this.LearnBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(6, 59);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(75, 23);
            this.stopBtn.TabIndex = 54;
            this.stopBtn.Text = "stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "kLearn";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "stop value";
            // 
            // stopLearnTextBox
            // 
            this.stopLearnTextBox.Location = new System.Drawing.Point(88, 59);
            this.stopLearnTextBox.Name = "stopLearnTextBox";
            this.stopLearnTextBox.Size = new System.Drawing.Size(48, 20);
            this.stopLearnTextBox.TabIndex = 37;
            this.stopLearnTextBox.TextChanged += new System.EventHandler(this.stopLearnTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "alpha";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "sLearn";
            // 
            // alphaTextBox
            // 
            this.alphaTextBox.Location = new System.Drawing.Point(88, 85);
            this.alphaTextBox.Name = "alphaTextBox";
            this.alphaTextBox.Size = new System.Drawing.Size(48, 20);
            this.alphaTextBox.TabIndex = 49;
            this.alphaTextBox.TextChanged += new System.EventHandler(this.alphaTextBox_TextChanged);
            // 
            // sLearnTextBox
            // 
            this.sLearnTextBox.Location = new System.Drawing.Point(88, 33);
            this.sLearnTextBox.Name = "sLearnTextBox";
            this.sLearnTextBox.Size = new System.Drawing.Size(48, 20);
            this.sLearnTextBox.TabIndex = 45;
            this.sLearnTextBox.TextChanged += new System.EventHandler(this.sLearnTextBox_TextChanged);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(6, 151);
            this.resultTextBox.Multiline = true;
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(365, 550);
            this.resultTextBox.TabIndex = 31;
            // 
            // captchaResultLabel
            // 
            this.captchaResultLabel.AutoSize = true;
            this.captchaResultLabel.Location = new System.Drawing.Point(387, 39);
            this.captchaResultLabel.Name = "captchaResultLabel";
            this.captchaResultLabel.Size = new System.Drawing.Size(51, 13);
            this.captchaResultLabel.TabIndex = 25;
            this.captchaResultLabel.Text = "unknown";
            // 
            // GenerateBtn
            // 
            this.GenerateBtn.Location = new System.Drawing.Point(377, 6);
            this.GenerateBtn.Name = "GenerateBtn";
            this.GenerateBtn.Size = new System.Drawing.Size(75, 23);
            this.GenerateBtn.TabIndex = 5;
            this.GenerateBtn.Text = "Generate";
            this.GenerateBtn.UseVisualStyleBackColor = true;
            this.GenerateBtn.Click += new System.EventHandler(this.GenerateBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(365, 139);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(995, 733);
            this.tabControl1.TabIndex = 7;
            // 
            // heightRange
            // 
            this.heightRange.Location = new System.Drawing.Point(377, 93);
            this.heightRange.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.heightRange.Name = "heightRange";
            this.heightRange.Size = new System.Drawing.Size(48, 20);
            this.heightRange.TabIndex = 68;
            this.heightRange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightRange.ValueChanged += new System.EventHandler(this.heightRange_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(431, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 67;
            this.label5.Text = "Разброс высоты";
            // 
            // lineCount
            // 
            this.lineCount.Location = new System.Drawing.Point(377, 119);
            this.lineCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.lineCount.Name = "lineCount";
            this.lineCount.Size = new System.Drawing.Size(48, 20);
            this.lineCount.TabIndex = 70;
            this.lineCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lineCount.ValueChanged += new System.EventHandler(this.lineCount_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(431, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 69;
            this.label9.Text = "Линий поверх";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 758);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.settingTabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitsNum)).EndInit();
            this.OutputTypePanel.ResumeLayout(false);
            this.OutputTypePanel.PerformLayout();
            this.ActivationFunctionPanel.ResumeLayout(false);
            this.ActivationFunctionPanel.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.heightRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox stopLearnTextBox;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label captchaResultLabel;
        private System.Windows.Forms.Button LearnAllBtn;
        private System.Windows.Forms.Button GenerateBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button saveNetBtn;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox sLearnTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox alphaTextBox;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button resetNetBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox netXTB;
        private System.Windows.Forms.TextBox netYTB;
        private System.Windows.Forms.TabControl settingTabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RadioButton BipolarSigmoid;
        private System.Windows.Forms.RadioButton Sigmoid;
        private System.Windows.Forms.Panel ActivationFunctionPanel;
        private System.Windows.Forms.TextBox kLearnTB;
        private System.Windows.Forms.Panel OutputTypePanel;
        private System.Windows.Forms.RadioButton Decimal;
        private System.Windows.Forms.RadioButton Binary;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown digitsNum;
        private System.Windows.Forms.NumericUpDown lineCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown heightRange;
        private System.Windows.Forms.Label label5;
    }
}

