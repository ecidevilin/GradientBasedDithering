using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public static partial class Matrix
    {
        public static float Dot(float[,] a, float[,] b)
        {
            float ret = 0;
            for (int i = 0, imax = a.GetLength(0); i < imax; i++)
            {
                for (int j = 0, jmax = a.GetLength(1); j < jmax; j++)
                {
                    ret += a[i, j] * b[i, j];
                }
            }
            return ret;
        }

        public static void TransposeChannel(int[] channel, int x, int y, int w, int h, ref float[,] a)
        {
            int imax = a.GetLength(1);
            int jmax = a.GetLength(0);
            int ioffset = -imax / 2;
            int joffset = -jmax / 2;
            for (int j = 0; j < jmax; j++)
            {
                for (int i = 0; i < imax; i++)
                {
                    int nx = x + i + joffset;
                    int ny = y + j + ioffset;
                    if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                    {
                        a[j, i] = 0;
                    }
                    else
                    {
                        a[j, i] = channel[ny * w + nx];
                    }
                }
            }
        }

        public static void TransposeChannel(int[] grayscale, int x, int y, int w, int h, ref Vector3[] a)
        {
            for (int j = 0; j <= 2; j++)
            {
                for (int i = 0; i <= 2; i++)
                {
                    int nx = x + i - 1;
                    int ny = y + j - 1;
                    if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                    {
                        a[j][i] = 0;
                    }
                    else
                    {
                        a[j][i] = grayscale[ny * w + nx];
                    }
                }
            }
        }
    }
}
