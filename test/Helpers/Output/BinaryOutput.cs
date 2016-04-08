using System;
using System.Linq;
using test.MyNet;

namespace test.Helpers.Output
{
    public class BinaryOutput : IOutput
    {
        private int _count = 1;

        public int SizeStek { get; } = 4;

        public int OutputNeurons { get; private set; } = 4;

        public int Count {
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

            for (var i = 0; i < OutputNeurons / 4; i++)
            {
                var part = output.ToList().Skip(i * 4).Take(4);
                var binValue = part.Aggregate<double, string>(null, (current, neuron) => current + (neuron > 0.5 ? "1" : "0"));
                var number = Convert.ToInt32(binValue, 2);

                res += number;
            }

            return res;
        }
    }
}
