using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CaptchaGenerator;
using test.Extensions;
using test.Helpers;
using test.Helpers.Output;
using test.MyNet;
using test.MyNet.Activation;

namespace test
{
    public partial class Form1 : Form
    {
        //создаем переменные
        private int _outputNeurons = 4;
        private double _kLearn = 0.01;
        private double _sLearn = 0.1;
        private int _stopSuccess = 80;
        private double _alpha = 1.0;
        private int _digitsCount = 1;
        private int _heightRange = 0;
        private int _lineCount = 0;
        private Net _net;
        private IActivation _activationFunction;
        private IOutput _outputFunction;

        private int _netSizeX = 10;
        private int _netSizeY = 10;

        public Form1()
        {
            InitializeComponent();
            kLearnTB.Text = _kLearn.ToString();
            stopLearnTextBox.Text = _stopSuccess.ToString();
            sLearnTextBox.Text = _sLearn.ToString();
            alphaTextBox.Text = _alpha.ToString();
            digitsNum.Value = _digitsCount;
            heightRange.Value = _heightRange;
            lineCount.Value = _lineCount;

            InitNet();
        }

        private void InitNet()
        {
            var activationType = GetActivationType();
            var outputType = GetOutputType();

            _activationFunction = ActivationFactory.Get(activationType);
            _outputFunction = OutputFactory.Get(outputType);
            _outputFunction.Count = _digitsCount;
            _outputNeurons = _outputFunction.OutputNeurons;

            var inputSize = 80 * 34;
            var layers = NetHelpers.GetLayerSizeList(_netSizeX, _netSizeY, _outputNeurons);

            _net = new Net(_activationFunction, inputSize, layers.ToArray());

            netXTB.Text = _net.SizeX.ToString();
            netYTB.Text = _net.SizeY.ToString();
        }

        private void saveNetBtn_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void openNetFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _net.OpenNw(openFileDialog3.FileName);

            netXTB.Text = _net.SizeX.ToString();
            netYTB.Text = _net.SizeY.ToString();

            var activation = _net.ActivationType.ToString();
            var rb = ActivationFunctionPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Name == activation);

            if (rb != null)
            {
                rb.Checked = true;
            }
        }

        private void loadNetBtn_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void saveNerFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _net.SaveNw(saveFileDialog1.FileName);
        }

        private double[] GetTemp(Bitmap bitmap)
        {
            Color Col;

            double[] temp = new double[(bitmap.Width) * (bitmap.Height)];

            for (int i = 0; i < (bitmap.Width); i++)
            {
                for (int j = 0; j < (bitmap.Height); j++)
                {
                    Col = bitmap.GetPixel(i, j);

                    var a = ((Convert.ToDouble(Col.R) / 255.0) + (Convert.ToDouble(Col.G) / 255.0) + (Convert.ToDouble(Col.B) / 255.0)) / 3;
                    var b = Convert.ToDouble(Col.B) / 255.0;

                    temp[i * (bitmap.Height) + j] = b;
                }
            }

            return temp;
        }

        private void MakeGray(Bitmap bmp)
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

        public Bitmap Contrast(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);


            double blue = 0;
            double green = 0;
            double red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


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


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        private double[] IntToDoubleArr(int value)
        {
            double[] digits = new double[_outputFunction.SizeStek];

            if (_outputFunction is BinaryOutput)
            {
                var binValue = Convert.ToString(value, 2).PadLeft(_outputFunction.SizeStek, '0');

                for (var i = 0; i < binValue.Length; i++)
                {
                    digits[i] = binValue[i] == '0' ? 0 : 1;
                }
            }
            else if (_outputFunction is BoolOutput)
            {
                for (var i = 0; i < _outputFunction.SizeStek; i++)
                {
                    digits[i] = i == value ? 1 : 0;
                }
            }
            else
            {
                for (var i = 0; i < _outputFunction.SizeStek; i++)
                {
                    digits[i] = i == value ? 0.5 : 0;
                }
            }

            return digits;
        }

        private double[] IntToDoubleArr(int value, double[] output)
        {
            double[] digits = new double[_outputFunction.SizeStek];

            if (_outputFunction is BinaryOutput)
            {
                var binValue = Convert.ToString(value, 2).PadLeft(_outputFunction.SizeStek, '0');

                for (var i = 0; i < binValue.Length; i++)
                {
                    if (binValue[i] == '1')
                    {
                        if (output[i] > 0.3 && output[i] < 0.5)
                        {
                            digits[i] = output[i];
                        }
                        else
                        {
                            digits[i] = 0.4;
                        }
                    }
                    else
                    {
                        if (output[i] < 0.3 && output[i] > 0.5)
                        {
                            digits[i] = output[i];
                        }
                        else
                        {
                            digits[i] = 0;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < _outputFunction.SizeStek; i++)
                {
                    digits[i] = i == value ? 1 : 0;
                }
            }

            return digits;
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            var c = Generator.Gen(_digitsCount, _heightRange, _lineCount);
            MakeGray(c.Image);
            var img = Contrast(c.Image, 100);

            var temp = GetTemp(img);
            var output = _outputFunction.Get(_net, temp);

            pictureBox1.BackgroundImage = img;
            captchaResultLabel.Text = output;
        }

        private async void LearnBtn_Click(object sender, EventArgs e)
        {
            _stopSuccess = _stopSuccess == 0 ? 80 : _stopSuccess;
            stopLearnTextBox.Text = _stopSuccess.ToString();
            resultTextBox.AppendText("Start learn");

            await Task.Run(() =>
            {
                if (_outputFunction is BoolOutput)
                {
                    LearnBoolType();
                }
                else
                {
                    Learn();
                }
            });
        }

        private void Learn(bool showLogs = true)
        {
            int success = 0;
            long i = 0;

            while (success < _stopSuccess)
            {
                var err = 0;
                var captcha = Generator.Gen(_digitsCount, _heightRange, _lineCount);

                MakeGray(captcha.Image);
                var img = Contrast(captcha.Image, 100);

                var temp = GetTemp(img);
                var output = _outputFunction.Get(_net, temp);
                var fOut = output;
                Stopwatch sw = new Stopwatch();

                while (output != captcha.Captcha[0].ToString())
                {
                    sw.Start();

                    var res = new List<double>();

                    foreach (var dVal in captcha.Captcha[0].ToString()
                        .Select(codeChar => Convert.ToInt32(codeChar.ToString()))
                        .Select(IntToDoubleArr))
                    {
                        res.AddRange(dVal);
                    }

                    var resArr = res.ToArray();

                    success = 0;
                    _net.Corr(temp, resArr, _kLearn);
                    output = _outputFunction.Get(_net, temp);
                    err++;
                    sw.Stop();

                    if (err > 3000)
                    {
                        break;
                    }
                }

                if (err == 0)
                {
                    success++;
                }

                if (showLogs)
                {
                    BeginInvoke(new EventHandler<LogEventArgs>(ShowLogs), this, new LogEventArgs(i, captcha.Captcha, fOut, err, success, sw.ElapsedMilliseconds));
                }

                i++;
            }
        }

        private void LearnBoolType(bool showLogs = true)
        {
            if (showLogs)
            {
                resultTextBox.AppendText("Start learn");
            }

            int success = 0;
            long i = 0;

            while (success < _stopSuccess)
            {
                var err = 0;

                if (showLogs)
                {
                    var myList = resultTextBox.Lines.ToList();

                    if (myList.Count > 40)
                    {
                        resultTextBox.Clear();
                    }
                }

                var captcha = Generator.Gen(_digitsCount, _heightRange, _lineCount);

                var temp = GetTemp(captcha.Image);
                var output = _outputFunction.Get(_net, temp);
                var fOut = output;
                Stopwatch sw = new Stopwatch();

                while ((captcha.Captcha == "2" && output == "0") || (captcha.Captcha != "2" && output == "1"))
                {
                    sw.Start();

                    double res = captcha.Captcha != "2" && output == "1" ? 0 : 0.5;
                    double[] resArr = { res };

                    success = 0;
                    _net.Corr(temp, resArr, _kLearn);
                    output = _outputFunction.Get(_net, temp);
                    err++;
                    sw.Stop();

                    if (err > 3000)
                    {
                        break;
                    }
                }

                if (err == 0)
                {
                    success++;
                }

                if (showLogs)
                {
                    BeginInvoke(new EventHandler<LogEventArgs>(ShowLogs), this, new LogEventArgs(i, captcha.Captcha, fOut, err, success, sw.ElapsedMilliseconds));
                }

                i++;
            }
        }

        private void ShowLogs(object sender, LogEventArgs e)
        {
            var myList = resultTextBox.Lines.ToList();

            if (myList.Count > 40)
            {
                resultTextBox.Clear();
            }

            resultTextBox.AppendText($"{Environment.NewLine}Iter {e.I}: code {e.Captcha} - answer {e.Answer}. errors {e.Errors}, success {e.Success}, time {e.Time}");
            resultTextBox.SelectionStart = resultTextBox.Text.Length;
            resultTextBox.ScrollToCaret();
        }

        private void stopLearnTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(stopLearnTextBox.Text, out _stopSuccess);
        }

        private void sLearnTextBox_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(sLearnTextBox.Text, out _sLearn);
        }

        private void alphaTextBox_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(alphaTextBox.Text, out _alpha);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            _stopSuccess = 0;
            stopLearnTextBox.Text = _stopSuccess.ToString();
        }

        private void resetNetBtn_Click(object sender, EventArgs e)
        {
            InitNet();
        }

        private void netYTB_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(netYTB.Text, out _netSizeY);
        }

        private void netXTB_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(netXTB.Text, out _netSizeX);
        }

        private ActivationType GetActivationType()
        {
            var checkedButton = ActivationFunctionPanel.Controls.OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked);

            return checkedButton != null
                ? EnumExtension.GetEnumByName<ActivationType>(checkedButton.Name)
                : ActivationType.BipolarSigmoid;
        }

        private OutputType GetOutputType()
        {
            var checkedButton = OutputTypePanel.Controls.OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked);

            return checkedButton != null
                ? EnumExtension.GetEnumByName<OutputType>(checkedButton.Name)
                : OutputType.Binary;
        }

        private void kLearnTB_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(kLearnTB.Text, out _kLearn);
        }

        private void digitsNum_ValueChanged(object sender, EventArgs e)
        {
             _digitsCount = Convert.ToInt32(digitsNum.Value);
            _outputNeurons = _outputFunction.SizeStek * _digitsCount;
        }

        private void heightRange_ValueChanged(object sender, EventArgs e)
        {
            _heightRange = Convert.ToInt32(heightRange.Value);
        }

        private void lineCount_ValueChanged(object sender, EventArgs e)
        {
            _lineCount = Convert.ToInt32(lineCount.Value);
        }
    }
}
