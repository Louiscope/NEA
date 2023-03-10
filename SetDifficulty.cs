using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SetDifficulty : MonoBehaviour
{

    public static double avg(double[] Scores)
    {
        return Scores.Where(x => x != 0).Average();

    }
    public double Calc(double P, double T, double Den, double Dev)
    {
        return System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * (P / 3100) - System.Math.Min(100, (System.Math.Pow(((0.4 * T) + 100) / 90, 3.0))) - (4 * Den) - (7 * Dev))))))));
    }

    public static double f(double S)
    {
        return System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * S)))))));
    }

}
