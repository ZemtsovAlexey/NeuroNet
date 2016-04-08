using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
                    temp[i * (bitmap.Height) + j] =
                        ((Convert.ToDouble(Col.B) + Convert.ToDouble(Col.G) + Convert.ToDouble(Col.R) + Convert.ToDouble(Col.A)) / (255.0 * 4));
                }
            }

            return temp;
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

            var temp = GetTemp(c.Image);
            var output = _outputFunction.Get(_net, temp);

            pictureBox1.BackgroundImage = c.Image;
            captchaResultLabel.Text = output;
        }

        private async void LearnBtn_Click(object sender, EventArgs e)
        {
            _stopSuccess = _stopSuccess == 0 ? 80 : _stopSuccess;
            stopLearnTextBox.Text = _stopSuccess.ToString();

            await Task.Run(() => Learn());
        }

        private async void Learn(bool showLogs = true)
        {
            if (showLogs)
            {
                resultTextBox.AppendText("Start learn");
            }

            int success = 0;
            int i = 0;

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

                while (output != captcha.Captcha)
                {
                    sw.Start();

                    var res = new List<double>();

                    foreach (var dVal in captcha.Captcha
                        .Select(codeChar => Convert.ToInt32(codeChar.ToString()))
                        .Select(IntToDoubleArr))
                    //.Select(intVal => IntToDoubleArr(intVal, _net.GetOut(temp))))
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
                    resultTextBox.AppendText($"{Environment.NewLine}Iter {i}: code {captcha.Captcha} - answer {fOut}. errors {err}, success {success}, time {sw.ElapsedMilliseconds}");
                    resultTextBox.SelectionStart = resultTextBox.Text.Length;
                    resultTextBox.ScrollToCaret();
                }

                i++;
            }
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
