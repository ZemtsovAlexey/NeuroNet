using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace CaptchaGenerator
{
    public static class Generator
    {
        private static Random rndFont = new Random();
        private static readonly string[] RundomColors = { "#505050", "#505050", "#505050" };
        const int imageHeight = 23;
        const int imageWidth = 16;

        public static CaptchaInfo Gen(int stringLength = 5, int randomHeight = 4, int numberLine = 12,
            int captchaWidth = 80, int captchaHeight = 30, string backColor = "#ececec", Random random = null)
        {
            Bitmap bmap = new Bitmap(captchaWidth, captchaHeight);
            Graphics grfs = Graphics.FromImage(bmap);
            var backGround = Color.FromArgb(0, 0, 0, 0);// ColorTranslator.FromHtml(backColor);
            grfs.Clear(backGround);

            random = random ?? new Random();

            Random rnd = new Random();
            var font = GetFont(random);

            //grfs.DrawLine(Pens.Black, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 50));
            //grfs.DrawRectangle(Pens.Blue, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(50, 80), rnd.Next(0, 20));
            //grfs.DrawLine(Pens.Red, rnd.Next(0, 50), rnd.Next(10, 30), rnd.Next(0, 200), rnd.Next(0, 20));

            //string str = "88642";//string.Format("{0}", rnd.Next(100000, 999999));
            //string str = "";
            //while (str.Length < stringLength)
            //{
            //    str += $"{rnd.Next(100000, 999999)}";
            //}
            //str = str.Substring(0, stringLength);
            string str = RandomString(stringLength);

            Graphics[] cGrfs = new Graphics[stringLength];
            SizeF[] sizes = new SizeF[stringLength];
            Bitmap[] cBmap = new Bitmap[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                char c = str[i];

                sizes[i] = grfs.MeasureString(c.ToString(), font,
                    new PointF(captchaWidth/(float) stringLength, captchaHeight), StringFormat.GenericTypographic);
                sizes[i].Width += 1;
                cBmap[i] = new Bitmap((int) Math.Round(sizes[i].Width), (int) Math.Round(sizes[i].Height));
                cGrfs[i] = Graphics.FromImage(cBmap[i]);
                cGrfs[i].Clear(backGround);

                Brush b = new SolidBrush(ColorTranslator.FromHtml("#505050"));

                cGrfs[i].TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                cGrfs[i].DrawString(c.ToString(), font, b, 0, 0, StringFormat.GenericTypographic);

                //for (int j = 0; j < rnd.Next(0, numberLine); j++)
                //{
                //    if (rnd.Next(0, 10) >= 1)
                //    {
                //        var y1 = rnd.Next(0, (int) sizes[i].Height);
                //        cGrfs[i].DrawLine(new Pen(backGround), 
                //            rnd.Next(0, (int) sizes[i].Width / 2), 
                //            y1,
                //            rnd.Next((int)sizes[i].Width / 2, (int) sizes[i].Width), 
                //            y1);
                //    }
                //    else
                //    {
                //        var y1 = rnd.Next(0, (int)sizes[i].Height / 2);
                //        var y2 = rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height);
                //        cGrfs[i].DrawLine(new Pen(backGround),
                //            rnd.Next(0, (int)sizes[i].Width / 2),
                //            y1,
                //            rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width),
                //            y2);

                //        /*cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width/2),
                //            rnd.Next(0, (int) sizes[i].Height/2), rnd.Next((int) sizes[i].Width/2, (int) sizes[i].Width),
                //            rnd.Next((int) sizes[i].Height/2, (int) sizes[i].Height));
                //        /*var x1 = rnd.Next(0, (int) sizes[i].Width);
                //        cGrfs[i].DrawLine(new Pen(backGround), x1,
                //            rnd.Next(0, (int) sizes[i].Height/2), x1,
                //            rnd.Next((int) sizes[i].Height/2, (int) sizes[i].Height));*/

                //    }
                //    //cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                //}
            }

            int x = (int) ((captchaWidth - sizes.Sum(s => s.Width))/2);
            int y = (int) ((captchaHeight - sizes.Max(s => s.Height))/2);

            for (int i = 0; i < stringLength; i++)
            {
                if (i > 0) x += 16;
                grfs.DrawImage(cBmap[i], x, y/* + rnd.Next(-randomHeight, randomHeight)*/);
            }

            //grfs.DrawString(str, fnt, Brushes.Green, 5, 5);
            // grfs.DrawString(text.Substring(0,3),fnt, Brushes.Navy, 20, 20);
            //grfs.DrawString(text.Substring(3),fnt, Brushes.Navy, 50, 20);
            //bmap.Save(Response.OutputStream, ImageFormat.Gif);

            return new CaptchaInfo(str, bmap);
        }

        public static CaptchaInfoEx GenEx(int stringLength = 5, int randomHeight = 4, int numberLine = 12,
            int captchaWidth = 80, int captchaHeight = 30, string backColor = "#ffffff", Random random = null)
        {
            List<Bitmap> splitBlocks = new List<Bitmap>();
            CaptchaInfo captchaInfo = new CaptchaInfo(null, null);
            int cicleCount = 0;
            random = random ?? new Random();

            while (splitBlocks.Count != stringLength)
            {
                captchaInfo = Gen(stringLength, randomHeight, numberLine, captchaWidth, captchaHeight, backColor, random);
                bool[] isBackColor;
                splitBlocks = SplitBlocks(captchaInfo.Image, ColorTranslator.FromHtml(backColor), 3, out isBackColor);
                if(cicleCount++ > 300) throw new Exception("Изображение не разбито на блоки за 300 циклов");
            }

            return new CaptchaInfoEx(captchaInfo.Captcha, captchaInfo.Image, splitBlocks, cicleCount);
        }

        public static List<Bitmap> SplitBlocks(Bitmap img, Color backColor, int minPixels, out bool [] isBackColor)
        {
            List<Bitmap> result= new List<Bitmap>();
            List<IntRange> blocks = new List<IntRange>();
            isBackColor = new bool[img.Width];
            //MakeGray(img);
            //Contrast(img, 100);
            //Blur(img);
            //Contrast(img, 100);

            for (int i = 0; i < img.Width; i++)
            {
                bool isVLineHaveBackColor = true;
                int findedPixels = 0;

                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j) != backColor)
                    {
                        findedPixels ++;
                    }

                    if (findedPixels >= minPixels)
                    {
                        isVLineHaveBackColor = false;
                        break;
                    }
                }

                isBackColor[i] = isVLineHaveBackColor;
            }

            if (!isBackColor[0])
            {
                isBackColor[0] = true;
            }

            IntRange range = new IntRange();

            for (int i = 1; i < isBackColor.Length; i++)
            {
                if (isBackColor[i - 1] != isBackColor[i])
                {
                    if (-1 == range.X1)
                    {
                        range.X1 = i - 1;
                    }
                    else if (-1 == range.X2)
                    {
                        range.X2 = i;
                    }
                    else
                    {
                        blocks.Add(range);
                        range = new IntRange();
                        range.X1 = i - 1;
                    }
                }
            }

            if (range.X1 != -1 && range.X2 != -1)
            {
                blocks.Add(range);
            }

            foreach (var block in blocks)
            {
                int vDelta = 0;
                for (int j = 0; j < img.Height; j++)
                {
                    bool isEmpty = true;

                    for (int i = block.X1; i <= block.X2; i++)
                    {
                        if (img.GetPixel(i, j) != backColor)
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                    if (!isEmpty)
                    {
                        vDelta = j;
                        break;
                    }
                }
                Rectangle rect = new Rectangle(block.X1, vDelta, block.X2 - block.X1, img.Height - vDelta >= imageHeight ? imageHeight : img.Height - vDelta);
                //Bitmap bitmap = new Bitmap(imageWidth, imageHeight); //img.Height);
                //using (Graphics g = Graphics.FromImage(bitmap))
                //{
                //    g.FillRectangle(new SolidBrush(backColor), 0, 0, bitmap.Width, bitmap.Height);
                //    g.DrawImage(img.Clone(rect, img.PixelFormat), new Point(0, 0));
                //}
                //result.Add(bitmap);
                result.Add(img.Clone(rect, img.PixelFormat));
            }

            return result;
        }

        public static List<Bitmap> SplitBlocksConst(Bitmap img)
        {
            Color backColor = img.GetPixel(0, 0);

            return SplitBlocksConst(img, backColor);
        }

        public static List<Bitmap> SplitBlocksConst(Bitmap img, Color backColor, bool expand = true)
        {
            List<Bitmap> result = new List<Bitmap>();
            IntRange[] ranges = { new IntRange {X1 = 0, X2 = 15}, new IntRange { X1 = 16, X2 = 31 }, new IntRange { X1 = 32, X2 = 47 }, new IntRange { X1 = 48, X2 = 63 }, new IntRange { X1 = 64, X2 = 79 } };

            foreach (var block in ranges)
            {
                int vDelta = 0;
                for (int j = 0; j < img.Height; j++)
                {
                    bool isEmpty = true;

                    for (int i = block.X1; i <= block.X2; i++)
                    {
                        if (img.GetPixel(i, j) != backColor)
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                    if (!isEmpty)
                    {
                        vDelta = j;
                        break;
                    }
                }

                Rectangle rect = new Rectangle(block.X1, vDelta, block.X2 - block.X1, img.Height - vDelta >= imageHeight ? imageHeight : img.Height - vDelta);
                Bitmap bmp;
                if (expand)
                {
                    bmp = ExpandTo15X23(img.Clone(rect, img.PixelFormat), backColor);
                }
                else
                {
                    bmp = img.Clone(rect, img.PixelFormat);
                }
                result.Add(bmp);
            }

            return result;
        }

        public static Bitmap ExpandTo15X23(Bitmap img, Color backColor)
        {
            const int minPixels = 1;
            int x = 0;

            for (int i = 0; i < img.Width; i++)
            {
                int findedPixels = 0;
                bool founded = false;

                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j).A == 255)
                    {
                        findedPixels++;
                    }

                    if (findedPixels >= minPixels)
                    {
                        founded = true;
                        break;
                    }
                }
                if (founded)
                {
                    x = i;
                    break;
                }
            }

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight); 
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(backColor), 0, 0, bitmap.Width, bitmap.Height);
                Rectangle rect = new Rectangle(x, 0, img.Width - x, img.Height);
                g.DrawImage(img.Clone(rect, img.PixelFormat), new Point(0, 0));
            }

            return bitmap;
        }

        public static Bitmap ExpandTo15X23Height(Bitmap img, Color backColor)
        {
            const int minPixels = 1;
            int x = 0;

            for (int i = 0; i < img.Width; i++)
            {
                int findedPixels = 0;
                bool founded = false;

                for (int j = 0; j < img.Height; j++)
                {
                    if (img.GetPixel(i, j).A == 255)
                    {
                        findedPixels++;
                    }

                    if (findedPixels >= minPixels)
                    {
                        founded = true;
                        break;
                    }
                }
                if (founded)
                {
                    x = i;
                    break;
                }
            }

            int y = 0;

            for (int i = 0; i < img.Height; i++)
            {
                int findedPixels = 0;
                bool founded = false;

                for (int j = 0; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i).A == 255)
                    {
                        findedPixels++;
                    }

                    if (findedPixels >= minPixels)
                    {
                        founded = true;
                        break;
                    }
                }
                if (founded)
                {
                    y = i;
                    break;
                }
            }

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(backColor), 0, 0, bitmap.Width, bitmap.Height);
                Rectangle rect = new Rectangle(x, y, img.Width - x, img.Height - y);
                g.DrawImage(img.Clone(rect, img.PixelFormat), new Point(0, 0));
            }

            return bitmap;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Font GetFont(Random random)
        {
            var r = random.Next(2);

            switch (r)
            {
                case 0:
                    return new Font("MV Boli", 18, FontStyle.Regular, GraphicsUnit.Point);

                case 1:
                    return new Font("Impact", 6.25f, FontStyle.Regular, GraphicsUnit.Millimeter);

                default: return new Font("MV Boli", 15, FontStyle.Bold);
            }
        }

        private class IntRange
        {
            public int X1 { get; set; }

            public int X2 { get; set; }

            public IntRange()
            {
                X1 = -1;
                X2 = -1;
            }
        }
    }
}
 