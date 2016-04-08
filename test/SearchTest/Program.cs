using System;
using System.Runtime.InteropServices;
using NeuroNet;
using NeuroNet.Activation;

namespace SearchTest
{
    class Program
    {
        private static bool run = true;
        private static IActivation _activation;
        private static Net net;

        static void Main(string[] args)
        {
            _activation = new BipolarSigmoidActivation();
            net = new Net(_activation, 1, 1, 1);

            string line = Console.ReadLine();

            while (run)
            {
                IfSearch(line);
                IfCorrect(line);

                line = Console.ReadLine();
            }
        }

        private static void IfSearch(string args)
        {
            if (args.StartsWith("-s "))
            {
                Console.WriteLine("start get");

                var text = args.TrimStart("-s ".ToCharArray());
                double res = 0;

                net.Context = new double[net.ContextSize];

                foreach (var ch in text)
                {
                    var a = Convert.ToByte(ch);
                    double[] input = new double[1];
                    input[0] = a * 0.01;
                    res = net.GetOut(input, true)[0];
                }

                Console.WriteLine($"{res}");
                Console.WriteLine("end get");
            }
        }

        private static void IfCorrect(string args)
        {
            if (args.StartsWith("-c "))
            {
                Console.WriteLine("start get");
                var arr = args.Split(';');

                var text = arr[0].TrimStart("-c ".ToCharArray());
                var corrVal = Convert.ToDouble(arr[1].Trim());
                double res = 0;

                for (int i = 0; i < text.Length; i++)
                {
                    var a = Convert.ToByte(text[i]);
                    double[] input = { a * 0.01 };
                    double[] r = {corrVal};

                    res = net.GetOut(input, true)[0];

                    if (i == text.Length - 1)
                    {
                        net.Corr(input, r);
                    }
                }

                foreach (var ch in text)
                {
                    var a = Convert.ToByte(ch);
                    double[] input = { a * 0.01 };

                    res = net.GetOut(input, true)[0];
                }

                Console.WriteLine($"{res}");
                Console.WriteLine("end get");
            }
        }
    }
}
