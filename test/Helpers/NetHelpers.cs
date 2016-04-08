using System.Collections.Generic;
using System.Windows.Forms;
using test.Helpers.Output;
using test.MyNet;
using test.MyNet.Activation;

namespace test.Helpers
{
    public static class NetHelpers
    {
        public static List<int> GetLayerSizeList(int x, int y, int output)
        {
            var layers = new List<int>() { x };

            for (var i = 0; i < y; i++)
            {
                layers.Add(x);
            }

            layers.Add(output);

            return layers;
        } 
    }
}
