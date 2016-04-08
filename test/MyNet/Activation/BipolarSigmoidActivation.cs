using System;

namespace test.MyNet.Activation
{
    public class BipolarSigmoidActivation : IActivation
    {
        public ActivationType Type { get; } = ActivationType.BipolarSigmoid;
        public double Alfa { get; set; } = 2;
        public double MaxRange => 1;
        public double MinRange => -1;

        public double Activation(double x)
        {
            return ((2 / (1 + Math.Exp(-Alfa * x))) - 1);
        }

        public double Derivative(double y)
        {
            return (Alfa * (1 - y * y) / 2);
        }

        public double DeActivation(double y)
        {
            return Math.Log((2 / (1 + y)) - 1);
        }
    }
}
