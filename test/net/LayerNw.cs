using System;

namespace test.net
{
    // Класс - слой нейросети
    public class LayerNw
    {
        public double[,] _weights;

        // Конструктор с параметрами. передается количество входных и выходных нейронов
        public LayerNw(int countX, int countY)
        {
            CountX = countX;
            CountY = countY;

            GiveMemory();
        }

        // Заполняем веса случайными числами
        public void GenerateWeights()
        {
            var rnd = new Random();

            for (var i = 0; i < CountX; i++)
            {
                for (var j = 0; j < CountY; j++)
                {
                    _weights[i, j] = rnd.NextDouble() - 0.5;
                }
            }
        }

        // Выделяет память под веса
        protected void GiveMemory()
        {
            _weights = new double[CountX, CountY];
        }

        public int CountX { get; }

        public int CountY { get; }

        public double this[int row, int col]
        {
            get { return _weights[row, col]; }
            set { _weights[row, col] = value; }
        }

    }
}
