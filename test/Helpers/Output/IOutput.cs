using AForge.Neuro;
using test.MyNet;

namespace test.Helpers.Output
{
    public interface IOutput
    {
        int OutputNeurons { get; }

        int SizeStek { get; }

        int Count { get; set; }

        string Get(Net net, double[] input);

        string GetAForge(ActivationNetwork net, double[] input);
    }
}
