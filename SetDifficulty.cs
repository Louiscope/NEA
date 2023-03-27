using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SetDifficulty : MonoBehaviour
{
    public double avg(double[] Scores)
    {
        return Scores.Average();
    }
    // Weighting algorithm
    public double Calc(double P, double T, double Den, double Dev)
    {
        return System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * (P / 3100) - System.Math.Min(100, (System.Math.Pow(((0.8 * T) + 100) / 90, 3.0))) - (4 * Den) - (7 * Dev))))))));
    }
    // Sigmoid curve -- Refer to docs for more info
    public int f(double S)
    {
        return Convert.ToInt32((System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * S)))))))/20)-1);
    }

}
