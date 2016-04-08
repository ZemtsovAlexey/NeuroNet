using System.Linq;
using test.MyNet;

namespace test.Helpers.Output
{
    public class DecimalOutput : IOutput
    {
        private int _count = 1;

        public int OutputNeurons { get; private set; } = 10;

        public int SizeStek { get; } = 10;

        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
                OutputNeurons = OutputNeurons * Count;
            }
        }

        public string Get(Net net, double[] input)
        {
            string res = null;
            var output = net.GetOut(input);

            for (int i = 0; i < OutputNeurons / SizeStek; i++)
            {
                int maxIter = 0;
                double maxRes = 0;

                var part = output.ToList().Skip(i * SizeStek).Take(SizeStek);
                var k = 0;

                foreach (var neuron in part)
                {
                    if (maxRes < neuron)
                    {
                        maxRes = neuron;
                        maxIter = k;
                    }

                    k++;
                }

                res += maxIter.ToString();
            }

            return res;
        }
    }
}
