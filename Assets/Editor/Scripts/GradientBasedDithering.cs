using UnityEngine;
using UnityEditor;

namespace Chaos
{
    partial class Dithering
    {
        public static void GradientBasedDithering5551(Texture2D texture, int smoothTimes, int threshold)
        {
            var width = texture.width;
            var height = texture.height;
            var resolution = width * height;

            var colors = texture.GetPixels();
            var offsets = 0;

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

            int[] grad = new int[resolution];
            int[] tempGrad = new int[resolution];

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

            offsets = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Color32 col = colors[offsets];
                    if (col.a < 1)
                    {
                        col.r = 0;
                        col.b = 0;
                        col.g = 7;
                        col.a = 0;
                        colors[offsets] = col;
                        offsets++;
                        continue;
                    }
                    if (!(grad[offsets] > threshold))
                    {
                        offsets++;
                        continue;
                    }

                    Dither5551(colors, offsets, width, height, x, y);
                    offsets++;
                }
            }

            texture.SetPixels(colors);
            EditorUtility.CompressTexture(texture, TextureFormat.RGB565, UnityEditor.TextureCompressionQuality.Best);
        }

    }
}

