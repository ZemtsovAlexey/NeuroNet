using System;

namespace test
{
    class Neuron
    {
        public int n = 1;
        public double lyambda;
        public double[] input;
        public double[] weight;
        public double output;

        public Neuron()
        {
            this.weight = new double[1];
            this.input = new double[1];
            this.lyambda = 1;
        }

        public Neuron(int n)
        {
            this.weight = new double[n];
            this.input = new double[n];
            this.lyambda = 1;
            this.n = n;
        }

        public void Activate()
        {
            double sum = 0;
            double net = 0;
            for (int i = 0; i < n; i++)
            {
                sum += input[i] * weight[i];
            }
            net = lyambda * sum;
            net = net / 1000;
            output = (Math.Exp(net) - Math.Exp(-net)) / (Math.Exp(net) + Math.Exp(-net));
            //output = 1 / (1 + Math.Exp(-lyambda * sum))-0.5;
            output = Math.Floor(output * 10000) / 10000;
        }

        /// Получение выхода нейрона.
        /// </summary>
        public double getOutput()
        {
            return output;
        }

        /// <summary>
        /// Установка весов
        /// </summary>
        /// <param name="temp"></param>
        public void setWeight()
        {
            for (int i = 0; i < n; i++)
            {
                weight[i] = 0.01;
            }
        }

        public void setInput(double[] temp)
        {
            for (int i = 0; i < n; i++)
            {
                input[i] = temp[i];
            }
        }

        public void setInput(double temp)
        {
            input[0] = temp;
        }
    }
}