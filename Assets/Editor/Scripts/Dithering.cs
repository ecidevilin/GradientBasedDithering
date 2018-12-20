using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Chaos
{
    partial class Dithering
    {

        public static void Dither(Texture2D texture, int smoothTimes, int threshold, int perCount, bool gradBased = true)
        {
            var width = texture.width;
            var height = texture.height;
            var resolution = width * height;

            var colors = texture.GetPixels();
            var offsets = 0;

            int[] grad = new int[resolution];
            int[] tempGrad = new int[resolution];
            if (gradBased)
            {
                int[] red = new int[resolution];
                int[] green = new int[resolution];
                int[] blue = new int[resolution];

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        Color32 c = colors[offsets];
                        red[offsets] = c.r;
                        green[offsets] = c.g;
                        blue[offsets] = c.b;
                        offsets++;
                    }
                }
                Filter.FilterByOperator(width, height, Filter.SobelGx, red, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                Filter.FilterByOperator(width, height, Filter.SobelGx, green, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                Filter.FilterByOperator(width, height, Filter.SobelGx, blue, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                Filter.FilterByOperator(width, height, Filter.SobelGy, red, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                Filter.FilterByOperator(width, height, Filter.SobelGy, green, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                Filter.FilterByOperator(width, height, Filter.SobelGy, blue, tempGrad);
                for (int i = 0; i < resolution; i++)
                {
                    grad[i] += Mathf.Abs(tempGrad[i]);
                }

                for (int i = 0; i < smoothTimes; i++)
                {
                    Filter.FilterByOperator(width, height, Gaussian.gaussion9, grad, tempGrad);
                    Utils.Swap(ref grad, ref tempGrad);
                }
            }

            offsets = 0;

            float[] param;
            if (perCount > 0)
                param = GetParameters(perCount);
            else
                param = new float[5];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (gradBased)
                    {
                        if (!(grad[offsets] > threshold))
                        {
                            offsets++;
                            continue;
                        }
                    }
                    if (perCount > 0)
                    {
                        DitherCustom(colors, offsets, width, height, x, y, param);
                    }
                    else if (perCount == -1)
                    {
                        Dither565(colors, offsets, width, height, x, y);
                    }
                    else if (perCount == -2)
                    {
                        Dither4444(colors, offsets, width, height, x, y);
                    }
                    offsets++;
                }
            }

            texture.SetPixels(colors);
            //EditorUtility.CompressTexture(texture, TextureFormat.RGB565, TextureCompressionQuality.Best);
        }

    }
}


