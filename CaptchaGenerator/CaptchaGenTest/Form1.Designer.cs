namespace CaptchaGet
{
    partial class Form1
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
            this._genBtn = new System.Windows.Forms.Button();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this._lenghtTbx = new System.Windows.Forms.TextBox();
            this._heightDurationTbx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._lineCountTbx = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._captchaWidthTbx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._captchaHeightTbx = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._foneTbx = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _genBtn
            // 
            this._genBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._genBtn.Location = new System.Drawing.Point(171, 307);
            this._genBtn.Name = "_genBtn";
            this._genBtn.Size = new System.Drawing.Size(101, 23);
            this._genBtn.TabIndex = 0;
            this._genBtn.Text = "Сгенерировать";
            this._genBtn.UseVisualStyleBackColor = true;
            this._genBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // _pictureBox
            // 
            this._pictureBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this._pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pictureBox.Location = new System.Drawing.Point(12, 12);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(164, 107);
            this._pictureBox.TabIndex = 1;
            this._pictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Длина:";
            // 
            // _lenghtTbx
            // 
            this._lenghtTbx.Location = new System.Drawing.Point(113, 135);
            this._lenghtTbx.Name = "_lenghtTbx";
            this._lenghtTbx.Size = new System.Drawing.Size(100, 20);
            this._lenghtTbx.TabIndex = 3;
            this._lenghtTbx.Text = "5";
            // 
            // _heightDurationTbx
            // 
            this._heightDurationTbx.Location = new System.Drawing.Point(113, 161);
            this._heightDurationTbx.Name = "_heightDurationTbx";
            this._heightDurationTbx.Size = new System.Drawing.Size(100, 20);
            this._heightDurationTbx.TabIndex = 5;
            this._heightDurationTbx.Text = "4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Разброс высоты:";
            // 
            // _lineCountTbx
            // 
            this._lineCountTbx.Location = new System.Drawing.Point(113, 187);
            this._lineCountTbx.Name = "_lineCountTbx";
            this._lineCountTbx.Size = new System.Drawing.Size(100, 20);
            this._lineCountTbx.TabIndex = 7;
            this._lineCountTbx.Text = "8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Линий поверх:";
            // 
            // _captchaWidthTbx
            // 
            this._captchaWidthTbx.Location = new System.Drawing.Point(113, 213);
            this._captchaWidthTbx.Name = "_captchaWidthTbx";
            this._captchaWidthTbx.Size = new System.Drawing.Size(100, 20);
            this._captchaWidthTbx.TabIndex = 9;
            this._captchaWidthTbx.Text = "80";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ширина поля:";
            // 
            // _captchaHeightTbx
            // 
            this._captchaHeightTbx.Location = new System.Drawing.Point(113, 239);
            this._captchaHeightTbx.Name = "_captchaHeightTbx";
            this._captchaHeightTbx.Size = new System.Drawing.Size(100, 20);
            this._captchaHeightTbx.TabIndex = 11;
            this._captchaHeightTbx.Text = "34";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Высота поля:";
            // 
            // _foneTbx
            // 
            this._foneTbx.Location = new System.Drawing.Point(113, 265);
            this._foneTbx.Name = "_foneTbx";
            this._foneTbx.Size = new System.Drawing.Size(100, 20);
            this._foneTbx.TabIndex = 13;
            this._foneTbx.Text = "#ececec";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Цвет фона:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 342);
            this.Controls.Add(this._foneTbx);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._captchaHeightTbx);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._captchaWidthTbx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._lineCountTbx);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._heightDurationTbx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._lenghtTbx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this._genBtn);
            this.Name = "Form1";
            this.Text = "Генератор каптча";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _genBtn;
        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _lenghtTbx;
        private System.Windows.Forms.TextBox _heightDurationTbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _lineCountTbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _captchaWidthTbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _captchaHeightTbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _foneTbx;
        private System.Windows.Forms.Label label6;
    }
}

