using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using test.MyNet.Activation;
using test.net;

namespace test.MyNet
{
    public class Net
    {
        private IActivation _activation;
        private Layer[] _layers;
        private int _countLayers;
        public double[] Context;

        public Layer[] Layers => _layers.Skip(1).Take(_layers.Length - 2).ToArray();
        public int SizeX => _layers.First().Outputs.Length;
        public int SizeY => Layers.Length;
        public ActivationType ActivationType => _activation.Type;

        public Net(IActivation activation, int x, params int[] layer)
        {
            _activation = activation;
            _layers = new Layer[layer.Length];
            _countLayers = _layers.Length;

            var random = new RandomNumbers();

            for (var k = 0; k < layer.Length; k++)
            {
                _layers[k] = new Layer(_activation, random, k == 0 ? x : layer[k - 1], layer[k], false);
            }

            Context = new double[SizeX];
        }

        public double[] GetOut(double[] x, bool recurrent = false)
        {
            if (recurrent)
            {
                double[] output = x;

                for (int i = 0; i < _layers.Length; i++)
                {
                    output = _layers[i].GetOut(output);

                    if (i == 0)
                    {
                        for (int o = 0; o < output.Length; o++)
                        {
                            output[o] += _activation.Activation(Context.Select((Xn, n) => _layers[i][o, n] * Xn).Sum());
                            Context[o] = output[o];
                        }
                    }
                }

                return output;
            }

            return _layers.Aggregate(x, (current, layer) => layer.GetOut(current));
        }

        public void Corr(double[] inputs, double[] rightResult, double kLern = 0.85)
        {
            GetOut(inputs);

            var lastLayer = _layers[_layers.Length - 1];

            for (var y = 0; y < lastLayer.CountY; y++)
            {
                var output = lastLayer.Outputs[y]; // Вход нейрона
                lastLayer.Delta[y] = (rightResult[y] - output) * _activation.Derivative(output);
            }

            // Перебираем все слои начиная споследнего 
            // изменяя веса и вычисляя дельта для скрытого слоя
            for (var k = _layers.Length - 1; k >= 0; k--)
            {
                // Изменяем веса выходного слоя
                CorrectLayerWeigths(_layers[k], kLern);

                //if (k == 0)
                //{
                //    currLayer.GetOut(inputs);

                //    for (var x = 0; x < currLayer.Outputs.Length; x++)
                //    {
                //        Context[x] = currLayer.Outputs[x];
                //    }
                //}

                if (k > 0)
                {
                    // Вычисляем дельта слоя к-1
                    CalcDelta(_layers[k - 1], _layers[k]);
                }
            }
        }

        private void CorrectLayerWeigths(Layer layer, double kLern)
        {
            for (var x = 0; x < layer.Inputs.Length; x++)
            {
                for (var y = 0; y < layer.CountY; y++)
                {
                    layer[y, x] += kLern * layer.Delta[y] * layer.Inputs[x];
                }
            }
        }

        private void CalcDelta(Layer prevLayer, Layer currLayer)
        {
            for (var j = 0; j < prevLayer.CountY; j++)
            {
                double sum = 0;

                for (var i = 0; i < currLayer.CountY; i++)
                {
                    sum += currLayer[i, j] * currLayer.Delta[i];
                }

                prevLayer.Delta[j] = _activation.Derivative(prevLayer.Outputs[j]) * sum;
            }
        }

        // Открывает НС
        public void OpenNw(string fileName)
        {
            byte[] binNw = File.ReadAllBytes(fileName);
            int k = 0;

            // Извлекаем тип активации
            var activationType = ReadFromArrayInt(binNw, ref k);
            _activation = ActivationFactory.Get((ActivationType)activationType);

            // Извлекаем количество слоев из массива
            _countLayers = ReadFromArrayInt(binNw, ref k);
            _layers = new Layer[_countLayers];

            // Извлекаем размерность слоев
            var countX1 = ReadFromArrayInt(binNw, ref k);
            var random = new RandomNumbers();

            for (var i = 0; i < _countLayers; i++)
            {
                var countY1 = ReadFromArrayInt(binNw, ref k);

                _layers[i] = new Layer(_activation, random, countX1, countY1, i == 0);
                countX1 = countY1;
            }

            // Извлекаем и записываем сами веса
            for (var l = 0; l < _countLayers; l++)
            {
                for (var y = 0; y < _layers[l].CountY; y++)
                {
                    for (var x = 0; x < _layers[l].CountX; x++)
                    {
                        _layers[l][y, x] = ReadFromArrayDouble(binNw, ref k);
                    }
                }
            }
        }

        // Сохраняет НС
        public void SaveNw(string fileName)
        {
            // размер сети в байтах
            var sizeNw = GetSizeNw();
            var binNw = new byte[sizeNw];

            var k = 0;

            // Записываем тип активации
            WriteInArray(binNw, ref k, (int)_activation.Type);

            // Записываем размерности слоев в массив байтов
            WriteInArray(binNw, ref k, _countLayers);

            if (_countLayers <= 0)
            {
                return;
            }

            WriteInArray(binNw, ref k, _layers[0].CountX);

            for (var i = 0; i < _countLayers; i++)
            {
                WriteInArray(binNw, ref k, _layers[i].CountY);
            }

            // Зпаисвыаем сами веса
            for (var l = 0; l < _countLayers; l++)
            {
                for (var y = 0; y < _layers[l].CountY; y++)
                {
                    for (var x = 0; x < _layers[l].CountX; x++)
                    {
                        WriteInArray(binNw, ref k, _layers[l][y, x]);
                    }
                }
            }


            File.WriteAllBytes(fileName, binNw);
        }

        // Возвращает размер НС в байтах
        private int GetSizeNw()
        {
            var sizeNw = sizeof(int) * (_countLayers + 3);

            for (var i = 0; i < _countLayers; i++)
            {
                sizeNw += sizeof(double) * _layers[i].CountX * _layers[i].CountY;
            }

            return sizeNw;
        }

        // Разбивает переменную типа int на байты и записывает в массив
        private void WriteInArray(byte[] mas, ref int pos, int value)
        {
            var dtb = new DataToByte { vInt = value };

            mas[pos++] = dtb.b1;
            mas[pos++] = dtb.b2;
            mas[pos++] = dtb.b3;
            mas[pos++] = dtb.b4;
        }

        // Разбивает переменную типа int на байты и записывает в массив
        private void WriteInArray(byte[] mas, ref int pos, double value)
        {
            var dtb = new DataToByte { vDouble = value };

            mas[pos++] = dtb.b1;
            mas[pos++] = dtb.b2;
            mas[pos++] = dtb.b3;
            mas[pos++] = dtb.b4;
            mas[pos++] = dtb.b5;
            mas[pos++] = dtb.b6;
            mas[pos++] = dtb.b7;
            mas[pos++] = dtb.b8;
        }

        // Извлекает переменную типа int из 4-х байтов массива
        private int ReadFromArrayInt(byte[] mas, ref int pos)
        {
            var dtb = new DataToByte
            {
                b1 = mas[pos++],
                b2 = mas[pos++],
                b3 = mas[pos++],
                b4 = mas[pos++]
            };

            return dtb.vInt;
        }

        // Извлекает переменную типа double из 8-ми байтов массива
        private double ReadFromArrayDouble(byte[] mas, ref int pos)
        {
            var dtb = new DataToByte
            {
                b1 = mas[pos++],
                b2 = mas[pos++],
                b3 = mas[pos++],
                b4 = mas[pos++],
                b5 = mas[pos++],
                b6 = mas[pos++],
                b7 = mas[pos++],
                b8 = mas[pos++]
            };

            return dtb.vDouble;
        }
    }
}
