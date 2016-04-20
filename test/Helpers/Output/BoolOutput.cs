using System.Linq;
using AForge.Neuro;
using test.MyNet;

namespace test.Helpers.Output
{
    public class BoolOutput : IOutput
    {
        private int _count = 1;

        public int OutputNeurons { get; private set; } = 1;

        public int SizeStek { get; } = 1;

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
                res += output[i] > 0.5 ? "1" : "0";
            }

            return res;
        }

        public string GetAForge(ActivationNetwork net, double[] input)
        {
            throw new System.NotImplementedException();
        }
    }
}
