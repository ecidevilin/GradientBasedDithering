using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public static partial class Filter
    {
        public static readonly Vector3[] SobelGx =
        {
            new Vector3(-1, 0, 1),
            new Vector3(-2, 0, 2),
            new Vector3(-1, 0, 1),
        };

        public static readonly Vector3[] SobelGy =
        {
            new Vector3(1, 2, 1),
            new Vector3(0, 0, 0),
            new Vector3(-1, -2, -1),
        };

        public static void FilterByOperator(int texw, int texh, Vector3[] op, int[] input, int[] output)
        {
            int offs = 0;
            Vector3[] amt = { Vector3.zero, Vector3.zero, Vector3.zero, };
            for (int y = 0; y < texh; y++)
            {
                for (int x = 0; x < texw; x++)
                {
                    Matrix.TransposeChannel(input, x, y, texw, texh, ref amt);
                    float rga = Vector3.Dot(amt[0], op[0]) + Vector3.Dot(amt[1], op[1]) + Vector3.Dot(amt[2], op[2]);
                    output[offs] = Mathf.RoundToInt(rga);
                    offs++;
                }
            }
        }

        public static void FilterByOperator(int texw, int texh, float[,] op, int[] input, int[] output)
        {
            int offs = 0;
            float[,] amt = new float[op.GetLength(0), op.GetLength(1)];
            for (int y = 0; y < texh; y++)
            {
                for (int x = 0; x < texw; x++)
                {
                    Matrix.TransposeChannel(input, x, y, texw, texh, ref amt);
                    float rga = Matrix.Dot(amt, op);
                    output[offs] = Mathf.RoundToInt(rga);
                    offs++;
                }
            }
        }
    }
}
