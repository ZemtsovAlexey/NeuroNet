using System.Linq;
using System.Threading.Tasks;
using test.MyNet.Activation;

namespace test.MyNet
{
    public class Layer
    {
        private double _alpha = 1;
        private readonly double _randomRangeMin = -1.0;
        private readonly double _randomRangeMax = 1.0;
        private readonly bool _isFirst = false;

        public IActivation Activation;

        public double[] Inputs;
        public double[,] Weigths;
        public double[] W;
        public double[] Outputs;
        public double[] Delta;
        public int CountX => Inputs.Length;
        public int CountY => Outputs.Length;

        public Layer(IActivation activation, RandomNumbers random, int x, int y, bool first = false)
        {
            _isFirst = first;
            Activation = activation;

            this.Inputs = new double[x];
            this.Weigths = new double[y, x];
            this.Outputs = new double[y];
            this.Delta = new double[y];

            _randomRangeMin = activation.MinRange;
            _randomRangeMax = activation.MaxRange;

            if (!first)
            {
                GenerateWeights(random);
            }
        }

        public double this[int y, int x]
        {
            get { return Weigths[y, x]; }
            set { Weigths[y, x] = value; }
        }

        private void GenerateWeights(RandomNumbers random)
        {
            var rnd = random;
            for (int l = 0; l < Outputs.Length; l++)
                for (var n = 0; n < Inputs.Length; n++)
                {
                    Weigths[l, n] = rnd.NextDouble(_randomRangeMin, _randomRangeMax);
                }
        }

        public double[] GetOut(double[] x)
        {
            Inputs = x;

            for (int y = 0; y < Outputs.Length; y++)
            {
                double e = Inputs.Select((Xn, n) => Weigths[y, n] * Xn).Sum();

                Outputs[y] = Activation.Activation(e);
            }

            return Outputs;
        }

        //private double Activate(IActivation activation, double e)
        //{
        //    //var a = Math.Tan(e);
        //    //var b = (Math.Exp(e) - Math.Exp(-e)) / (Math.Exp(e) + Math.Exp(-e));

        //    //var sum = 1.0f / (1.0f + Math.Exp(-e));
        //    //var sum2 = (2.0 / (1.0 + Math.Exp(-e))) - 1;
        //    //var sum22 = (2.0 / (1.0 + Math.Exp(-2*e))) - 1;
        //    //var sum3 = Math.Sin(e);
        //    //var sum4 = Math.Sqrt(e);

        //    return activation.ActivationType(e);
        //}
    }
}
