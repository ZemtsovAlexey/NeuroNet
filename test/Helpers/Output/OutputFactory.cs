using System;

namespace test.Helpers.Output
{
    public static class OutputFactory
    {
        public static IOutput Get(OutputType type)
        {
            IOutput outputFunction = null;

            switch (type)
            {
                case OutputType.Binary:
                    outputFunction = new BinaryOutput();
                    break;

                case OutputType.Decimal:
                    outputFunction = new DecimalOutput();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return outputFunction;
        }
    }
}
