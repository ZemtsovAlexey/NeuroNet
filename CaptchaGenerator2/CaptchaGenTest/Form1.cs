using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CaptchaGenerator;

namespace CaptchaGet
{
    public partial class Form1 : Form
    {
        private string [] _rundomColors = new [] { "#0000FF", "#000080", "#008000" };
        const int StringLength = 5;
        const int CaptchaWidth = 80;
        const int CaptchaHeight = 34;
        const int RandomHeight = 4;
        const int NumberLine = 8;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            BeginInvoke(new EventHandler<EventArgs>(button1_Click), new object[] {this, EventArgs.Empty});
            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Bitmap bmap = new Bitmap(CaptchaWidth, CaptchaHeight);
            Graphics grfs = Graphics.FromImage(bmap);
            var backGround = ColorTranslator.FromHtml(_foneTbx.Text);
            grfs.Clear(backGround);
            Font fnt = new Font("Times New Roman", 17, FontStyle.Bold);

            Random rnd = new Random();
            //grfs.DrawLine(Pens.Black, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 50));
            //grfs.DrawRectangle(Pens.Blue, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(50, 80), rnd.Next(0, 20));
            //grfs.DrawLine(Pens.Red, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 20));

            string str = "88642";//string.Format("{0}", rnd.Next(100000, 999999));
            //string str = string.Format("{0}", rnd.Next((int) Math.Pow(10, StringLength), (int) Math.Pow(10, StringLength+1))).Substring(0, StringLength);

            Graphics[] cGrfs = new Graphics[StringLength];
            SizeF[] sizes = new SizeF[StringLength];
            Bitmap[] cBmap = new Bitmap[StringLength];

            for (int i = 0; i < StringLength; i++)
            {
                char c = str[i];

                sizes[i] = grfs.MeasureString(c.ToString(), fnt, new PointF(CaptchaWidth / (float)StringLength, CaptchaHeight), StringFormat.GenericTypographic);
                sizes[i].Width += 1;
                cBmap[i] = new Bitmap((int) Math.Round(sizes[i].Width), (int) Math.Round(sizes[i].Height));
                cGrfs[i] = Graphics.FromImage(cBmap[i]);
                cGrfs[i].Clear(backGround);

                Brush b = new SolidBrush(ColorTranslator.FromHtml(_rundomColors[rnd.Next(0, 3)]));

                cGrfs[i].DrawString(c.ToString(), fnt, b, 0, 0, StringFormat.GenericTypographic);

                for (int j = 0; j < rnd.Next(0, NumberLine); j++)
                {
                    if (rnd.Next(0, 10) > 5)
                    {
                        var y1 = rnd.Next(0, (int) sizes[i].Height/2);
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width/2), y1,
                            rnd.Next((int) sizes[i].Width/2, (int) sizes[i].Width), y1);
                    }
                    else
                    {
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int)sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                    }
                    //cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                }
            }

            int x = (int) ((CaptchaWidth - sizes.Sum(s => s.Width)) / 2);
            int y = (int)((CaptchaHeight - sizes.Max(s => s.Height)) / 2);

            for (int i = 0; i < StringLength; i++)
            {
                if (i > 0) x += (int)sizes[i - 1].Width;
                grfs.DrawImage(cBmap[i], x, y + rnd.Next(-RandomHeight, RandomHeight));
            }

            //grfs.DrawString(str, fnt, Brushes.Green, 5, 5);
            // grfs.DrawString(text.Substring(0,3),fnt, Brushes.Navy, 20, 20);
            //grfs.DrawString(text.Substring(3),fnt, Brushes.Navy, 50, 20);
            //bmap.Save(Response.OutputStream, ImageFormat.Gif);*/

            if (_pictureBox.Width < int.Parse(_captchaWidthTbx.Text))
                _pictureBox.Width = int.Parse(_captchaWidthTbx.Text) + 20;

            var captchaInfo = Generator.Gen(int.Parse(_lenghtTbx.Text), int.Parse(_heightDurationTbx.Text), int.Parse(_lineCountTbx.Text), int.Parse(_captchaWidthTbx.Text), int.Parse(_captchaHeightTbx.Text), _foneTbx.Text);
            _pictureBox.Image = captchaInfo.Image;
            Text = captchaInfo.Captcha + " - Генератор каптча";

            SplitImageToBlocks(captchaInfo.Image);
        }

        private void SplitImageToBlocks(Bitmap bitmap)
        {
            bool[] isBackColor;
            List<Bitmap> list = Generator.SplitBlocks(bitmap, ColorTranslator.FromHtml(_foneTbx.Text), int.Parse(_pixelsTbx.Text), out isBackColor);
            PictureBox[] pbList = new[]
            {pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7};
            for (int i = 0; i < list.Count; i++)
            {
                if (pbList.Length > i)
                {
                    pbList[i].Image = list[i];
                }
            }
            for (int i = list.Count; i < pbList.Length; i++)
            {
                pbList[i].Image = new Bitmap(1, 1);
            }

            _listBox.Items.Clear();
            for (int i = 0; i < isBackColor.Length; i++)
            {
                _listBox.Items.Add(i + " - " + isBackColor[i]);
            }
        }

        private void _btnLoad_Click(object sender, EventArgs e)
        {
            _pictureBox.Load(_urlTbx.Text);
            SplitImageToBlocks((Bitmap) _pictureBox.Image);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _infoStatusLbl.Text = ((Bitmap) (((PictureBox) sender).Image)).Width.ToString();
        }

        private void _pixelsTbx_TextChanged(object sender, EventArgs e)
        {
            SplitImageToBlocks((Bitmap) _pictureBox.Image);
        }

        private void _genBtnSimple_Click(object sender, EventArgs e)
        {
            var infoEx = Generator.GenEx(int.Parse(_lenghtTbx.Text), int.Parse(_heightDurationTbx.Text), int.Parse(_lineCountTbx.Text), int.Parse(_captchaWidthTbx.Text), int.Parse(_captchaHeightTbx.Text), _foneTbx.Text);
            List<Bitmap> list = infoEx.Blocks;
            PictureBox[] pbList = {pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7};
            for (int i = 0; i < list.Count; i++)
            {
                if (pbList.Length > i)
                {
                    pbList[i].Image = list[i];
                }
            }
            for (int i = list.Count; i < pbList.Length; i++)
            {
                pbList[i].Image = new Bitmap(1, 1);
            }
            _listBox.Items.Clear();
        }
    }
}
