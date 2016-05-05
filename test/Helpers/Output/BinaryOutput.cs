using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using test.MyNet;

namespace test.Helpers.Output
{
    public class BinaryOutput : IOutput
    {
        private int _count = 1;

        public int SizeStek { get; } = 8;

        public int OutputNeurons { get; private set; } = 8;

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

            for (var i = 0; i < OutputNeurons / SizeStek; i++)
            {
                var part = output.ToList().Skip(i * SizeStek).Take(SizeStek);
                var binValue = part.Aggregate<double, string>(null, (current, neuron) => current + (neuron > 0.5 ? "1" : "0"));
                var data = GetBytesFromBinaryString(binValue);
                var number = Encoding.ASCII.GetString(data);// Convert.ToInt32(binValue, 2);

                res += number;
            }

            return res;
        }

        private Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }

        public string GetAForge(ActivationNetwork net, double[] input)
        {
            throw new NotImplementedException();
        }
    }
}
