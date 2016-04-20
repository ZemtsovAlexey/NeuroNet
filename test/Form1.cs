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
using System.Windows.Forms.DataVisualization.Charting;
using AForge.Neuro;
using AForge.Neuro.Learning;
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
        private int _sLearn = 1000;
        private int _stopSuccess = 80;
        private double _alpha = 1.0;
        private int _digitsCount = 1;
        private int _heightRange = 0;
        private int _lineCount = 0;
        private bool _showLogs = true;
        private Net _net;
        private Net _net2;
        private Net _net3;
        private Net _net4;
        private IActivation _activationFunction;
        private IOutput _outputFunction;
        private Series _seriesStop;
        private Series _seriesSuccess;
        private Series _seriesErrors;
        private int _succeses;
        private int _errors;
        private ActivationNetwork _network;
        private DistanceNetwork _distanceNetwork;

        private int _netSizeX = 15;
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
            _activationFunction.Alfa = _alpha;
            _outputFunction = OutputFactory.Get(outputType);
            _outputFunction.Count = _digitsCount;
            _outputNeurons = _outputFunction.OutputNeurons;

            var inputSize = 15 * 23;
            var layers = NetHelpers.GetLayerSizeList(_netSizeX, _netSizeY, _outputNeurons);
            var random = new RandomNumbers();
            _net = new Net(random, _activationFunction, inputSize, layers.ToArray());
            _net2 = new Net(random, _activationFunction, inputSize, layers.ToArray());
            _net3 = new Net(random, _activationFunction, inputSize, layers.ToArray());
            _net4 = new Net(random, _activationFunction, inputSize, layers.ToArray());

            _network = new ActivationNetwork(new SigmoidFunction(_alpha), inputSize, layers.ToArray());
            _distanceNetwork = new DistanceNetwork(inputSize, 10);

            netXTB.Text = _net.SizeX.ToString();
            netYTB.Text = _net.SizeY.ToString();

            chart1.Series.Clear();

            _seriesStop = new Series
            {
                Name = "Stop",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2
            };

            _seriesSuccess = new Series
            {
                Name = "Success",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 3
            };

            _seriesErrors = new Series
            {
                Name = "Error",
                Color = Color.Red,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 3
            };

            this.chart1.Series.Add(_seriesStop);
            this.chart1.Series.Add(_seriesSuccess);
            this.chart1.Series.Add(_seriesErrors);
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

        private void openNet2FileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _net2.OpenNw(openFileDialog1.FileName);
        }

        private void loadNet2Btn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openNet3FileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _net3.OpenNw(openFileDialog3.FileName);
        }

        private void loadNet3Btn_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openNet4FileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _net4.OpenNw(openFileDialog3.FileName);
        }

        private void loadNet4Btn_Click(object sender, EventArgs e)
        {
            openFileDialog4.ShowDialog();
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

        public void Contrast(Bitmap sourceBitmap, int threshold)
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

        private void Blur(Bitmap bm)
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
                    digits[i] = i == value ? 1 : 0;
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

            List<Bitmap> list = new List<Bitmap>();
            int cicleCount = 0;

            while (list.Count != 5)
            {
                pictureBox1.Load("http://195.16.122.162/osrweb/UserControls/Captcha.ashx?ClientID=Captcha_ctl00_ContentPlaceHolderBody_TabMain_PanelSearch_CaptchaDigit_ImageCaptcha&FontName=Times+New+Roman&CaptchaColor=%23ececec&ColorLine=%23ececec&ColorBorder=&FontColor=&%D0%A1haracterSet=1234567890&FontSize=18&CaptchaLength=5&%D0%A1aptchaWidth=80&%D0%A1aptchaHeight=34&RandomHeight=4&NumberLine=8&time=07.04.2016%2010:43:01");
                bool[] isBackColor;
                list = Generator.SplitBlocks((Bitmap)pictureBox1.Image, ColorTranslator.FromHtml("#ffffff"), 3, out isBackColor);
                Application.DoEvents();
                if (cicleCount++ > 20)
                {
                    MessageBox.Show("Не разбито на блоки за 20 циклов");
                    return;
                }
            }

            //var list = Generator.GenEx(5, _heightRange, _lineCount).Blocks;

            Blur(list[0]);
            Contrast(list[0], 100);
            pictureBox1.Image = list[0];

            var temp = GetTemp(list[0]);
            var output = _outputFunction.Get(_net, temp);
            var output2 = _outputFunction.Get(_net2, temp);
            var output3 = _outputFunction.Get(_net3, temp);
            var output4 = _outputFunction.GetAForge(_network, temp);

            captchaResultLabel.Text = output;
            captchaResultLabel2.Text = output2;
            captchaResultLabel3.Text = output3;
            captchaResultLabel4.Text = output4;
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

        private void Learn()
        {
            int success = 0;
            long iteration = 0;

            while (success < _stopSuccess)
            {
                var errors = 0;
                var captcha = Generator.GenEx(5, _heightRange, _lineCount);

                for (var k = 0; k < captcha.Blocks.Count; k++)
                {
                    var temp = GetTemp(captcha.Blocks[k]);
                    var answer = _outputFunction.Get(_net, temp);
                    var firstAnswer = answer;
                    
                    while (answer != captcha.Captcha[k].ToString())
                    {
                        var res = new List<double>();

                        foreach (var dVal in captcha.Captcha[k].ToString()
                            .Select(x => Convert.ToInt32(x.ToString()))
                            .Select(IntToDoubleArr))
                        {
                            res.AddRange(dVal);
                        }

                        var resArr = res.ToArray();

                        success = 0;
                        
                        _net.Correct(temp, resArr, _kLearn);
                        answer = _outputFunction.Get(_net, temp);
                        errors++;

                        if (errors > 3000)
                        {
                            break;
                        }
                    }

                    if (errors == 0)
                    {
                        success++;
                    }

                    if (_showLogs)
                    {
                        BeginInvoke(new EventHandler<LogEventArgs>(ShowLogs), this,
                            new LogEventArgs(iteration, captcha.Captcha[k].ToString(), firstAnswer, errors, success));
                    }

                    iteration++;
                }
            }
        }

        private void LearnAForge()
        {
            int success = 0;
            long iteration = 0;

            while (success < _stopSuccess)
            {
                var errors = 0;
                var captcha = Generator.GenEx(5, _heightRange, _lineCount);

                for (var k = 0; k < captcha.Blocks.Count; k++)
                {
                    var temp = GetTemp(captcha.Blocks[k]);
                    var answer = _outputFunction.GetAForge(_network, temp);
                    var firstAnswer = answer;

                    // create teacher
                    var teacher = new BackPropagationLearning(_network);
                    //teacher.Momentum = 0.0;
                    teacher.LearningRate = _kLearn;
                    double e = 0;

                    while (answer != captcha.Captcha[k].ToString())
                    {
                        var res = new List<double>();

                        foreach (var dVal in captcha.Captcha[k].ToString()
                            .Select(x => Convert.ToInt32(x.ToString()))
                            .Select(IntToDoubleArr))
                        {
                            res.AddRange(dVal);
                        }

                        var resArr = res.ToArray();

                        success = 0;

                        e = teacher.Run(temp, resArr);
                        answer = _outputFunction.GetAForge(_network, temp);
                        errors++;

                        //if (errors > 3000)
                        //{
                        //    break;
                        //}
                    }

                    if (errors == 0)
                    {
                        success++;
                    }

                    if (_showLogs)
                    {
                        BeginInvoke(new EventHandler<LogEventArgs>(ShowLogs), this,
                            new LogEventArgs(iteration, captcha.Captcha[k].ToString(), firstAnswer, errors, success));
                    }

                    iteration++;
                }
            }
        }

        //private void LearnNet(Net net, double[] temp, string captchaAnser)
        //{
        //    var answer = _outputFunction.Get(_net, temp);

        //    while (answer != captchaAnser)
        //    {
        //        var res = new List<double>();

        //        foreach (var dVal in captchaAnser
        //            .Select(x => Convert.ToInt32(x.ToString()))
        //            .Select(IntToDoubleArr))
        //        {
        //            res.AddRange(dVal);
        //        }

        //        var resArr = res.ToArray();

        //        success = 0;

        //        _net.Correct(temp, resArr, _kLearn);
        //        answer = _outputFunction.Get(_net, temp);
        //        errors++;

        //        if (errors > 3000)
        //        {
        //            break;
        //        }
        //    }
        //}

        private void TestNet()
        {
            int errors = 0;
            long iteration = 0;

            while (iteration < _sLearn / 5)
            {
                var captcha = Generator.GenEx(5, _heightRange, _lineCount);

                for (var k = 0; k < captcha.Blocks.Count; k++)
                {
                    var ans = new List<string>();

                    var temp = GetTemp(captcha.Blocks[k]);
                    ans.Add(_outputFunction.Get(_net, temp));
                    ans.Add(_outputFunction.Get(_net2, temp));
                    ans.Add(_outputFunction.Get(_net3, temp));
                    ans.Add(_outputFunction.Get(_net4, temp));

                    var answer = ans.GroupBy(x => x).OrderByDescending(x => x.Count()).First();

                    if (answer.Key != captcha.Captcha[k].ToString())
                    {
                        errors++;
                    }

                    iteration++;
                }
            }

            if (_showLogs)
            {
                BeginInvoke(new EventHandler<LogEventArgs>(ShowTestNetLogs), this, new LogEventArgs(iteration, string.Empty, string.Empty, errors, 0));
            }
        }

        private void LearnBoolType()
        {
            if (_showLogs)
            {
                resultTextBox.AppendText("Start learn");
            }

            int success = 0;
            long i = 0;

            while (success < _stopSuccess)
            {
                var err = 0;

                if (_showLogs)
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
                    _net.Correct(temp, resArr, _kLearn);
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

                if (_showLogs)
                {
                    BeginInvoke(new EventHandler<LogEventArgs>(ShowLogs), this, new LogEventArgs(i, captcha.Captcha, fOut, err, success));
                }

                i++;
            }
        }

        private void ShowGraffic(long iteration, int success, int errors)
        {
            _succeses = success > _succeses 
                ? success 
                : _succeses <= 0 
                    ? 0 
                    : success == 0 
                        ? _succeses - 3
                        : _succeses;

            _errors = errors > _errors
                ? errors
                : _errors <= 0
                    ? 0
                    : _errors - 1;

            if (_seriesStop.Points.Count > 60)
            {
                _seriesStop.Points.Remove(_seriesStop.Points.First());
                _seriesSuccess.Points.Remove(_seriesSuccess.Points.First());
                _seriesErrors.Points.Remove(_seriesErrors.Points.First());
            }

            _seriesStop.Points.AddXY(iteration, _stopSuccess);
            _seriesSuccess.Points.AddXY(iteration, _succeses);
            _seriesErrors.Points.AddXY(iteration, _errors);
        }

        private void ShowLogs(object sender, LogEventArgs e)
        {
            ShowGraffic(e.I, e.Success, e.Errors);

            var myList = resultTextBox.Lines.ToList();

            if (myList.Count > 40)
            {
                resultTextBox.Clear();
            }

            resultTextBox.AppendText($"{Environment.NewLine}Iter {e.I}: code {e.Captcha} - answer {e.Answer}. errors {e.Errors}, success {e.Success}");
            resultTextBox.SelectionStart = resultTextBox.Text.Length;
            resultTextBox.ScrollToCaret();
        }

        private void ShowTestNetLogs(object sender, LogEventArgs e)
        {
            var myList = resultTextBox.Lines.ToList();

            if (myList.Count > 40)
            {
                resultTextBox.Clear();
            }

            resultTextBox.AppendText($"{Environment.NewLine}From {e.I} itertions - {e.Errors} errors");
            resultTextBox.SelectionStart = resultTextBox.Text.Length;
            resultTextBox.ScrollToCaret();
        }

        private void stopLearnTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(stopLearnTextBox.Text, out _stopSuccess);
            this.chart1.ResetAutoValues();
        }

        private void sLearnTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(sLearnTextBox.Text, out _sLearn);
        }

        private void alphaTextBox_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(alphaTextBox.Text, out _alpha);
            //_activationFunction.Alfa = _alpha;
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
            //_outputNeurons = _outputFunction.SizeStek * _digitsCount;
        }

        private void heightRange_ValueChanged(object sender, EventArgs e)
        {
            _heightRange = Convert.ToInt32(heightRange.Value);
        }

        private void lineCount_ValueChanged(object sender, EventArgs e)
        {
            _lineCount = Convert.ToInt32(lineCount.Value);
        }

        private void ShowLogsBtn_Click(object sender, EventArgs e)
        {
            _showLogs = !_showLogs;
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            this.chart1.ResetAutoValues();
        }

        private async void TestBtn_Click(object sender, EventArgs e)
        {
            resultTextBox.AppendText($"{Environment.NewLine}Start test");

            await Task.Run(() =>
            {
                TestNet();
            });
        }
    }
}
