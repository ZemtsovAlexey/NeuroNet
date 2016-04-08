using System;

public class RandomNumbers : Random
{
    public RandomNumbers() : base() { }

    public double NextDouble(double minimum, double maximum)
    {
        return base.NextDouble() * (maximum - minimum) + minimum;
    }
}

