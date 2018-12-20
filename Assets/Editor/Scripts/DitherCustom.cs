using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chaos
{
    partial class Dithering
    {
        public static float[] GetParameters(int perCount)
        {
            if (perCount < bitsPerB)
                throw new System.Exception("抖动位数小于4位");

            var @params = new float[5];
            int delta = perCount - bitsPerB;
            var powDelta = Mathf.Pow(2, delta);
            @params[0] = k1b * powDelta;
            @params[1] = k3b * powDelta;
            @params[2] = k5b * powDelta;
            @params[3] = k7b * powDelta;
            if (delta != 0)
            {
                @params[0] -= 1;
                @params[1] -= 1;
                @params[2] += 1;
                @params[3] += 1;
            }
            float curPerB = perB * powDelta;
            for (int i = 0; i < 4; i++)
            {
                @params[i] /= curPerB;
            }
            @params[4] = curPerB;
            return @params;
        }
        public static void DitherCustom(Color[] colors, int offsets, int width, int height, int x, int y, float[] @params)
        {
            float rd = colors[offsets].r;
            float gn = colors[offsets].g;
            float bl = colors[offsets].b;

            var rd2 = Mathf.Clamp01(Mathf.Floor(rd * @params[4]) / (@params[4] - 1));
            var gn2 = Mathf.Clamp01(Mathf.Floor(gn * @params[4]) / (@params[4] - 1));
            var bl2 = Mathf.Clamp01(Mathf.Floor(bl * @params[4]) / (@params[4] - 1));

            var rde = rd - rd2;
            var gne = gn - gn2;
            var ble = bl - bl2;

            var right = offsets + 1;
            var lowerLeft = offsets + width - 1;
            var lower = offsets + width;
            var lowerRight = offsets + width + 1;

            if (x < width - 1)
            {
                colors[right].r += rde * @params[3];//k7
                colors[right].g += gne * @params[3];//k7
                colors[right].b += ble * @params[3];//k7
            }

            if (y < height - 1)
            {
                colors[lower].r += rde * @params[2];//k5
                colors[lower].g += gne * @params[2];//k5
                colors[lower].b += ble * @params[2];//k5

                if (x > 0)
                {
                    colors[lowerLeft].r += rde * @params[1];//k3
                    colors[lowerLeft].g += gne * @params[1];//k3
                    colors[lowerLeft].b += ble * @params[1];//k3
                }

                if (x < width - 1)
                {
                    colors[lowerRight].r += rde * @params[0];//k1
                    colors[lowerRight].g += gne * @params[0];//k1
                    colors[lowerRight].b += ble * @params[0];//k1
                }
            }

            colors[offsets].r = rd2;
            colors[offsets].g = gn2;
            colors[offsets].b = bl2;
        }

    }
}
