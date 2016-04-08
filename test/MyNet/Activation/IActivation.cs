namespace test.MyNet.Activation
{
    public interface IActivation
    {
        ActivationType Type { get; }

        double Alfa { get; set; }

        double MaxRange { get; }

        double MinRange { get; }

        double Activation(double x);

        double Derivative(double y);

        double DeActivation(double y);
    }
}