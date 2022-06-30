using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MathUtils{

    public static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * ((t >= 0.0 && t <= 1.0) ? t : t < 0.0 ? 0.0 : 1.0);
    }

    public static double RandRange(double minimum, double maximum)
    { 
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}
