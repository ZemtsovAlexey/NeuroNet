using System;

namespace NeuroNet.Extensions
{
    public class RandomNumbers : Random
    {
        public double NextDouble(double minimum, double maximum)
        {
            return base.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}

