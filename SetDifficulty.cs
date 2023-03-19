using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SetDifficulty : MonoBehaviour
{
    // Takes in the 3 values from Scores in FinishLevel.cs and calculates an average between them
    public double avg(double[] Scores)
    {
        return Scores.Average();
    }
    // Provides the initial weighting calculations for the difficulty
    public double Calc(double P, double T, double Den, double Dev)
    {
        return System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * (P / 3100) - System.Math.Min(100, (System.Math.Pow(((0.8 * T) + 100) / 90, 3.0))) - (4 * Den) - (7 * Dev))))))));
    }
    // Reciprocal Exponential curve which gives values from 0 to 5 for difficulty which is used in RandomGen.cs
    public int f(double S)
    {
        return Convert.ToInt32((System.Math.Floor(Convert.ToDouble(10 * ((int)System.Math.Round(10 / (1 + (System.Math.Exp(-0.1 * S)))))))/20)-1);
    }

}
