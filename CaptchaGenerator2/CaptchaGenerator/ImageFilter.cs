using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CaptchaGenerator
{
    public class ImageFilter
    {
        public static void Blur(Bitmap bm)
        {
            int w = bm.Width;
            int h = bm.Height;

            // horizontal blur
            for (int i = 1; i < w - 1; i++)
                for (int j = 0; j < h; j++)
                {
                    Color c1 = bm.GetPixel(i - 1, j);
                    Color c2 = bm.GetPixel(i, j);
                    Color c3 = bm.GetPixel(i + 1, j);


                    byte bR = (byte)((c1.R + c2.R + c3.R) / 3);
                    byte bG = (byte)((c1.G + c2.G + c3.G) / 3);
                    byte bB = (byte)((c1.B + c2.B + c3.B) / 3);


                    Color cBlured = Color.FromArgb(bR, bG, bB);

                    bm.SetPixel(i, j, cBlured);

                }

            //vertical blur
            for (int i = 0; i < w; i++)
                for (int j = 1; j < h - 1; j++)
                {
                    Color c1 = bm.GetPixel(i, j - 1);
                    Color c2 = bm.GetPixel(i, j);
                    Color c3 = bm.GetPixel(i, j + 1);


                    byte bR = (byte)((c1.R + c2.R + c3.R) / 3);
                    byte bG = (byte)((c1.G + c2.G + c3.G) / 3);
                    byte bB = (byte)((c1.B + c2.B + c3.B) / 3);


                    Color cBlured = Color.FromArgb(bR, bG, bB);

                    bm.SetPixel(i, j, cBlured);

                }
        }

        public static void MakeGray(Bitmap bmp)
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
    }
}
