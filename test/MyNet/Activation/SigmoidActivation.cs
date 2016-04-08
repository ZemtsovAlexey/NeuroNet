using System;

namespace test.MyNet.Activation
{
    public class SigmoidActivation : IActivation
    {
        public ActivationType Type { get; } = ActivationType.Sigmoid;
        public double Alfa { get; set; } = 2;
        public double MaxRange => 1;
        public double MinRange => 0;

        public double Activation(double x)
        {
            return (1 / (1 + Math.Exp(-Alfa * x)));
        }

        public double Derivative(double y)
        {
            return (Alfa * y * (1 - y));
        }

        public double DeActivation(double y)
        {
            throw new NotImplementedException();
        }
    }
}
