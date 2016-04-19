using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace CaptchaGenerator
{
    public static class Generator
    {
        private static readonly string[] RundomColors = {"#0000FF", "#000080", "#008000"};

        public static CaptchaInfo Gen(int stringLength = 5, int randomHeight = 4, int numberLine = 8,
            int captchaWidth = 80, int captchaHeight = 34, string backColor = "#ececec")
        {
            Bitmap bmap = new Bitmap(captchaWidth, captchaHeight);
            Graphics grfs = Graphics.FromImage(bmap);
            var backGround = ColorTranslator.FromHtml(backColor);
            grfs.Clear(backGround);
            Font fnt = new Font("Times New Roman", 17, FontStyle.Bold);

            Random rnd = new Random();
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

                sizes[i] = grfs.MeasureString(c.ToString(), fnt,
                    new PointF(captchaWidth/(float) stringLength, captchaHeight), StringFormat.GenericTypographic);
                sizes[i].Width += 1;
                cBmap[i] = new Bitmap((int) Math.Round(sizes[i].Width), (int) Math.Round(sizes[i].Height));
                cGrfs[i] = Graphics.FromImage(cBmap[i]);
                cGrfs[i].Clear(backGround);

                Brush b = new SolidBrush(ColorTranslator.FromHtml(RundomColors[rnd.Next(0, 3)]));

                cGrfs[i].DrawString(c.ToString(), fnt, b, 0, 0, StringFormat.GenericTypographic);

                for (int j = 0; j < rnd.Next(0, numberLine); j++)
                {
                    if (rnd.Next(0, 10) > 5)
                    {
                        var y1 = rnd.Next(0, (int) sizes[i].Height/2);
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width/2), y1,
                            rnd.Next((int) sizes[i].Width/2, (int) sizes[i].Width), y1);
                    }
                    else
                    {
                        cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width/2),
                            rnd.Next(0, (int) sizes[i].Height/2), rnd.Next((int) sizes[i].Width/2, (int) sizes[i].Width),
                            rnd.Next((int) sizes[i].Height/2, (int) sizes[i].Height));
                        /*var x1 = rnd.Next(0, (int) sizes[i].Width);
                        cGrfs[i].DrawLine(new Pen(backGround), x1,
                            rnd.Next(0, (int) sizes[i].Height/2), x1,
                            rnd.Next((int) sizes[i].Height/2, (int) sizes[i].Height));*/
                    }
                    //cGrfs[i].DrawLine(new Pen(backGround), rnd.Next(0, (int) sizes[i].Width / 2), rnd.Next(0, (int)sizes[i].Height / 2), rnd.Next((int)sizes[i].Width / 2, (int)sizes[i].Width), rnd.Next((int)sizes[i].Height / 2, (int)sizes[i].Height));
                }
            }

            int x = (int) ((captchaWidth - sizes.Sum(s => s.Width))/2);
            int y = (int) ((captchaHeight - sizes.Max(s => s.Height))/2);

            for (int i = 0; i < stringLength; i++)
            {
                if (i > 0) x += (int) sizes[i - 1].Width;
                grfs.DrawImage(cBmap[i], x, y + rnd.Next(-randomHeight, randomHeight));
            }

            //grfs.DrawString(str, fnt, Brushes.Green, 5, 5);
            // grfs.DrawString(text.Substring(0,3),fnt, Brushes.Navy, 20, 20);
            //grfs.DrawString(text.Substring(3),fnt, Brushes.Navy, 50, 20);
            //bmap.Save(Response.OutputStream, ImageFormat.Gif);

            return new CaptchaInfo(str, bmap);
        }

        public static CaptchaInfoEx GenEx(int stringLength = 5, int randomHeight = 4, int numberLine = 8,
            int captchaWidth = 80, int captchaHeight = 34, string backColor = "#ffffff")
        {
            List<Bitmap> splitBlocks = new List<Bitmap>();
            CaptchaInfo captchaInfo = null;

            while (splitBlocks.Count != stringLength)
            {
                captchaInfo = Gen(stringLength, randomHeight, numberLine, captchaWidth, captchaHeight, backColor);
                bool[] isBackColor;
                splitBlocks = SplitBlocks(captchaInfo.Image, ColorTranslator.FromHtml(backColor), 3, out isBackColor);
            }

            Debug.Assert(captchaInfo != null, "captchaInfo != null");
            return new CaptchaInfoEx(captchaInfo.Captcha, captchaInfo.Image, splitBlocks);
        }

        public static List<Bitmap> SplitBlocks(Bitmap img, Color backColor, int minPixels, out bool [] isBackColor)
        {
            const int ImageHeight = 23;
            const int ImageWidth = 15;

            List<Bitmap> result= new List<Bitmap>();
            List<IntRange> blocks = new List<IntRange>();
            isBackColor = new bool[img.Width];
            MakeGray(img);
            Contrast(img, 100);

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
                Rectangle rect = new Rectangle(block.X1, vDelta, block.X2 - block.X1, img.Height - vDelta >= ImageHeight ? ImageHeight : img.Height - vDelta);
                Bitmap bitmap = new Bitmap(ImageWidth, ImageHeight); //img.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.FillRectangle(new SolidBrush(backColor), 0, 0, bitmap.Width, bitmap.Height);
                    g.DrawImage(img.Clone(rect, img.PixelFormat), new Point(0, 0));
                }
                result.Add(bitmap);
                //result.Add(img.Clone(rect, img.PixelFormat);
            }

            return result;
        }


        private static void MakeGray(Bitmap bmp)
        {
            // Задаём формат Пикселя.
            PixelFormat pxf = PixelFormat.Format24bppRgb;

            // Получаем данные картинки.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //Блокируем набор данных изображения в памяти
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;

            // Задаём массив из Byte и помещаем в него надор данных.
            // int numBytes = bmp.Width * bmp.Height * 3; 
            //На 3 умножаем - поскольку RGB цвет кодируется 3-мя байтами
            //Либо используем вместо Width - Stride
            int numBytes = bmpData.Stride * bmp.Height;
            int widthBytes = bmpData.Stride;
            byte[] rgbValues = new byte[numBytes];

            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Перебираем пикселы по 3 байта на каждый и меняем значения
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {

                int value = rgbValues[counter] + rgbValues[counter + 1] + rgbValues[counter + 2];
                byte color_b = 0;

                color_b = Convert.ToByte(value / 3);


                rgbValues[counter] = color_b;
                rgbValues[counter + 1] = color_b;
                rgbValues[counter + 2] = color_b;

            }
            // Копируем набор данных обратно в изображение
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Разблокируем набор данных изображения в памяти.
            bmp.UnlockBits(bmpData);
        }

        public static void Contrast(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            double blue = 0;
            double green = 0;
            double red = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;

                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }

                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }

                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            Marshal.Copy(pixelBuffer, 0, sourceData.Scan0, pixelBuffer.Length);
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