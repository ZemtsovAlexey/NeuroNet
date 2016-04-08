namespace NeuroNet.Activation
{
    public interface IActivation
    {
        ActivationType Type { get; }

        double Alfa { get; set; }

        double Range { get; }

        double Activation(double x);

        double Derivative(double y);

        double DeActivation(double y);
    }
}