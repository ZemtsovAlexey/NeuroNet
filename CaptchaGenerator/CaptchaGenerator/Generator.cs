using System;
using System.Drawing;
using System.Linq;

namespace CaptchaGenerator
{
    public static class Generator
    {
        private static readonly string[] RundomColors = { "#0000FF", "#000080", "#008000" };
        private static Random rnd = new Random();

        public static CaptchaInfo Gen(int stringLength = 5, int randomHeight = 4, int numberLine = 8, int captchaWidth = 20, int captchaHeight = 34, string backColor = "#ececec")
        {
            //stringLength = 5;

            Bitmap bmap = new Bitmap(captchaWidth, captchaHeight);
            Graphics grfs = Graphics.FromImage(bmap);
            var backGround = ColorTranslator.FromHtml(backColor);
            grfs.Clear(backGround);
            Font fnt = new Font("Times New Roman", 17, FontStyle.Bold);

            //grfs.DrawLine(Pens.Black, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 50));
            //grfs.DrawRectangle(Pens.Blue, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(50, 80), rnd.Next(0, 20));
            //grfs.DrawLine(Pens.Red, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 20));

            //string str = "88642";//string.Format("{0}", rnd.Next(100000, 999999));
            string str = "";
            while (str.Length < stringLength)
            {
                str += $"{rnd.Next(100000, 999999)}";
            }
            str = str.Substring(0, stringLength);

            Graphics[] cGrfs = new Graphics[stringLength];
            SizeF[] sizes = new SizeF[stringLength];
            Bitmap[] cBmap = new Bitmap[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                char c = str[i];

                sizes[i] = grfs.MeasureString(c.ToString(), fnt, new PointF(captchaWidth / (float)stringLength, captchaHeight), StringFormat.GenericTypographic);
                sizes[i].Width += 1;
                cBmap[i] = new Bitmap((int)Math.Round(sizes[i].Width), (int)Math.Round(sizes[i].Height));
                cGrfs[i] = Graphics.FromImage(cBmap[i]);
                cGrfs[i].Clear(backGround);

                Brush b = new SolidBrush(ColorTranslator.FromHtml(RundomColors[rnd.Next(0, 3)]));

                cGrfs[i].DrawString(c.ToString(), fnt, b, 0, 0, StringFormat.GenericTypographic);

                for (int j = 0; j < rnd.Next(0, numberLine); j++)
                {
                    if (rnd.Next(0, 10) > 5)
                    {
                        var y1 = rnd.Next(0, (int)sizes[i].Height / 2);
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int)sizes[i].Width / 2), y1,
                            rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), y1);
                    }
                    else
                    {
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int)sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                        /*var x1 = rnd.Next(0, (int) sizes[i].Width);
                        cGrfs[i].DrawLine(new Pen(backGround), x1,
                            rnd.Next(0, (int) sizes[i].Height/2), x1,
                            rnd.Next((int) sizes[i].Height/2, (int) sizes[i].Height));*/
                    }
                    //cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                }
            }

            int x = (int)((captchaWidth - sizes.Sum(s => s.Width)) / 2);
            int y = (int)((captchaHeight - sizes.Max(s => s.Height)) / 2);

            for (int i = 0; i < stringLength; i++)
            {
                if (i > 0) x += (int)sizes[i - 1].Width;
                grfs.DrawImage(cBmap[i], x, y + rnd.Next(-randomHeight, randomHeight));
            }

            //grfs.DrawString(str, fnt, Brushes.Green, 5, 5);
            // grfs.DrawString(text.Substring(0,3),fnt, Brushes.Navy, 20, 20);
            //grfs.DrawString(text.Substring(3),fnt, Brushes.Navy, 50, 20);
            //bmap.Save(Response.OutputStream, ImageFormat.Gif);

            return new CaptchaInfo(str, bmap);
        }
    }
}
