using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Helper
{
    public static Vector3 ToVector3(this double[] vector) => new Vector3((float)vector[0], (float)vector[1], (float)vector[2]);

    public static Color ToColor(this double[] vector) =>
        vector.Length <= 3 ?
            new Color((float)vector[0], (float)vector[1], (float)vector[2])
            : new Color((float)vector[0], (float)vector[1], (float)vector[2], (float)vector[3]);

    public static Color ReplaceAlpha(this Color color, float a)
    {
        Color n = color; n.a = a; return n;
    }
}
