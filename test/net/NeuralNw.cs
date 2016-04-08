using System;
using System.Collections.Generic;
using System.IO;

namespace test.net
{
    // Класс - нейронная сеть
    public class NeuralNw
    {
        public LayerNw[] _layers;
        int _countLayers, _countX, _countY;
        double[][] _netout;  // NETOUT[countLayers + 1][]
        double[][] _delta;   // NETOUT[countLayers    ][]

        // Конструкторы
        /* Создает полносвязанную сеть из 1 слоя. 
           sizeX - размерность вектора входных параметров
           sizeY - размерность вектора выходных параметров */
        public NeuralNw(int sizeX, int sizeY)
        {
            _countLayers = 1;
            _layers = new LayerNw[_countLayers];
            _layers[0] = new LayerNw(sizeX, sizeY);
            _layers[0].GenerateWeights();
        }

        /* Создает полносвязанную сеть из n слоев. 
           sizeX - размерность вектора входных параметров
           layers - массив слоев. Значение элементов массива - количество нейронов в слое               
         */
        public NeuralNw(int sizeX, params int[] layers)
        {
            _countLayers = layers.Length;
            _countX = sizeX;
            _countY = layers[layers.Length - 1];
            // Размерность выходов нейронов и Дельты
            _netout = new double[_countLayers + 1][];
            _netout[0] = new double[sizeX];
            _delta = new double[_countLayers][];

            _layers = new LayerNw[_countLayers];

            int countY1, countX1 = sizeX;

            // Устанавливаем размерность слоям и заполняем слоя случайными числами
            for (int i = 0; i < _countLayers; i++)
            {
                countY1 = layers[i];

                _netout[i + 1] = new double[countY1];
                _delta[i] = new double[countY1];

                _layers[i] = new LayerNw(countX1, countY1);
                _layers[i].GenerateWeights();
                countX1 = countY1;
            }
        }

        // Открывает НС
        public NeuralNw(string fileName)
        {
            OpenNw(fileName);
        }

        // Открывает НС
        public void OpenNw(string fileName)
        {
            byte[] binNw = File.ReadAllBytes(fileName);
            int k = 0;

            // Извлекаем количество слоев из массива
            _countLayers = ReadFromArrayInt(binNw, ref k);
            _layers = new LayerNw[_countLayers];

            // Извлекаем размерность слоев
            int countY1 = 0, countX1 = ReadFromArrayInt(binNw, ref k);

            // Размерность входа
            _countX = countX1;

            // Выделяемпамять под выходы нейронов и дельта
            _netout = new double[_countLayers + 1][];
            _netout[0] = new double[countX1];
            _delta = new double[_countLayers][];

            for (int i = 0; i < _countLayers; i++)
            {
                countY1 = ReadFromArrayInt(binNw, ref k);
                _layers[i] = new LayerNw(countX1, countY1);
                countX1 = countY1;

                // Выделяем память
                _netout[i + 1] = new double[countY1];
                _delta[i] = new double[countY1];
            }

            // Размерность выхода
            _countY = countY1;

            // Извлекаем и записываем сами веса
            for (int r = 0; r < _countLayers; r++)
            {
                for (int p = 0; p < _layers[r].CountX; p++)
                {
                    for (int q = 0; q < _layers[r].CountY; q++)
                    {
                        _layers[r][p, q] = ReadFromArrayDouble(binNw, ref k);
                    }
                }
            }
        }

        // Сохраняет НС
        public void SaveNw(string fileName)
        {
            // размер сети в байтах
            int sizeNw = GetSizeNw();
            byte[] binNw = new byte[sizeNw];

            int k = 0;
            // Записываем размерности слоев в массив байтов
            WriteInArray(binNw, ref k, _countLayers);

            if (_countLayers <= 0)
            {
                return;
            }

            WriteInArray(binNw, ref k, _layers[0].CountX);

            for (int i = 0; i < _countLayers; i++)
            {
                WriteInArray(binNw, ref k, _layers[i].CountY);
            }

            // Зпаисвыаем сами веса
            for (int r = 0; r < _countLayers; r++)
            {
                for (int p = 0; p < _layers[r].CountX; p++)
                {
                    for (int q = 0; q < _layers[r].CountY; q++)
                    {
                        WriteInArray(binNw, ref k, _layers[r][p, q]);
                    }
                }
            }


            File.WriteAllBytes(fileName, binNw);
        }

        // Возвращает значение j-го слоя НС
        public void NetOut(double[] inX, out double[] outY, int jLayer)
        {
            GetOut(inX, jLayer);

            int n = _netout[jLayer].Length;

            outY = new double[n];

            for (int i = 0; i < n; i++)
            {
                outY[i] = _netout[jLayer][i];
            }

        }

        // Возвращает значение НС
        public void NetOut(double[] inX, out double[] outY)
        {
            int j = _countLayers;

            NetOut(inX, out outY, j);
        }

        // Возвращает ошибку (метод наименьших квадратов)
        public double CalcError(double[] x, double[] y)
        {
            double kErr = 0;

            for (int i = 0; i < y.Length; i++)
            {
                kErr += Math.Pow(y[i] - _netout[_countLayers][i], 2);
            }

            return 0.5 * kErr;
        }

        /* Обучает сеть, изменяя ее весовые коэффициэнты. 
           Inputs, Outputs - обучающая пара. kLern - скорость обучаемости
           В качестве результата метод возвращает ошибку 0.5(Outputs-outY)^2 */
        public double LernNw(double[] x, double[] y, double kLern)
        {
            // Вычисляем выход сети
            GetOut(x);

            // Заполняем дельта последнего слоя
            for (int j = 0; j < _layers[_countLayers - 1].CountY; j++)
            {
                var o = _netout[_countLayers][j];  // Вход нейрона
                _delta[_countLayers - 1][j] = (y[j] - o) * o * (1 - o);
            }

            // Перебираем все слои начиная споследнего 
            // изменяя веса и вычисляя дельта для скрытого слоя
            for (int k = _countLayers - 1; k >= 0; k--)
            {
                // Изменяем веса выходного слоя

                for (int i = 0; i < _layers[k].CountX; i++)
                {
                    for (int j = 0; j < _layers[k].CountY; j++)
                    {
                        _layers[k][i, j] += kLern * _delta[k][j] * _netout[k][i];
                    }
                }

                if (k > 0)
                {
                    // Вычисляем дельта слоя к-1
                    for (int j = 0; j < _layers[k - 1].CountY; j++)
                    {
                        double s = 0;

                        for (int i = 0; i < _layers[k].CountY; i++)
                        {
                            s += _layers[k][j, i] * _delta[k][i];
                        }

                        _delta[k - 1][j] = _netout[k][j] * (1 - _netout[k][j]) * s;
                    }
                }
            }

            return CalcError(x, y);
        }

        // Свойства. Возвращает число входов и выходов сети
        public int GetX
        {
            get { return _countX; }
        }

        public int GetY
        {
            get { return _countY; }
        }

        public int CountLayers
        {
            get { return _countLayers; }
        }
        /* Вспомогательные закрытые функции */

        // Возвращает все значения нейронов до lastLayer слоя
        void GetOut(double[] inX, int lastLayer)
        {
            double s;

            for (int j = 0; j < _layers[0].CountX; j++)
            {
                _netout[0][j] = inX[j];
            }

            for (int i = 0; i < lastLayer; i++)
            {
                // размерность столбца проходящего через i-й слой
                for (int j = 0; j < _layers[i].CountY; j++)
                {
                    s = 0;

                    for (int k = 0; k < _layers[i].CountX; k++)
                    {
                        s += _layers[i][k, j] * _netout[i][k];
                    }

                    // Вычисляем значение активационной функции
                    s = 1.0 / (1 + Math.Exp(-s));
                    _netout[i + 1][j] = 0.998 * s + 0.001;
                }
            }
        }

        // Возвращает все значения нейронов всех слоев
        void GetOut(double[] inX)
        {
            GetOut(inX, _countLayers);
        }

        // Возвращает размер НС в байтах
        int GetSizeNw()
        {
            int sizeNw = sizeof(int) * (_countLayers + 2);

            for (int i = 0; i < _countLayers; i++)
            {
                sizeNw += sizeof(double) * _layers[i].CountX * _layers[i].CountY;
            }

            return sizeNw;
        }

        // Возвращает num-й слой Нейронной сети
        public LayerNw Layer(int num)
        {
            return _layers[num];
        }

        // Разбивает переменную типа int на байты и записывает в массив
        void WriteInArray(byte[] mas, ref int pos, int value)
        {
            var dtb = new DataToByte { vInt = value };

            mas[pos++] = dtb.b1;
            mas[pos++] = dtb.b2;
            mas[pos++] = dtb.b3;
            mas[pos++] = dtb.b4;
        }

        // Разбивает переменную типа int на байты и записывает в массив
        void WriteInArray(byte[] mas, ref int pos, double value)
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
        int ReadFromArrayInt(byte[] mas, ref int pos)
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
        double ReadFromArrayDouble(byte[] mas, ref int pos)
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
