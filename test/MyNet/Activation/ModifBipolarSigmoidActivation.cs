using System;

namespace test.MyNet.Activation
{
    public static class ModifBipolarSigmoidActivation
    {
        public static double Alpha = 2;

        public static double Activation(double x)
        {
            return ((2 / (1 + Math.Exp(-Alpha * x))) - 1);
        }

        public static double Derivative(double x)
        {
            double y = Activation(x);

            return (Alpha * (1 - y * y) / 2);
        }

        public static double Derivative2(double y)
        {
            return (Alpha * (1 - y * y) / 2);
        }
    }
}
