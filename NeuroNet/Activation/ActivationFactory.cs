using System;

namespace NeuroNet.Activation
{
    public static class ActivationFactory
    {
        public static IActivation Get(ActivationType activationType, double alpha = 1)
        {
            IActivation activationFunction = null;

            switch (activationType)
            {
                case ActivationType.Sigmoid:
                    activationFunction = new SigmoidActivation();
                    break;

                case ActivationType.BipolarSigmoid:
                    activationFunction = new BipolarSigmoidActivation();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(activationType), activationType, null);
            }

            activationFunction.Alfa = alpha;

            return activationFunction;
        }
    }
}
