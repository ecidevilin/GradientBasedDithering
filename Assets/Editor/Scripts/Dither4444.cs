using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    partial class Dithering
    {

        //Dither4444纹理压缩方案
        public static void Dither4444(Color[] colors, int offsets, int width, int height, int x, int y)
        {
            float al = colors[offsets].a;
            float rd = colors[offsets].r;
            float gn = colors[offsets].g;
            float bl = colors[offsets].b;

            var al2 = Mathf.Clamp01(Mathf.Floor(al * 16) * k1p15);
            var rd2 = Mathf.Clamp01(Mathf.Floor(rd * 16) * k1p15);
            var gn2 = Mathf.Clamp01(Mathf.Floor(gn * 16) * k1p15);
            var bl2 = Mathf.Clamp01(Mathf.Floor(bl * 16) * k1p15);

            var ale = al - al2;
            var rde = rd - rd2;
            var gne = gn - gn2;
            var ble = bl - bl2;

            colors[offsets].a = al2;
            colors[offsets].r = rd2;
            colors[offsets].g = gn2;
            colors[offsets].b = bl2;

            var right = offsets + 1;
            var lowerLeft = offsets + width - 1;
            var lower = offsets + width;
            var lowerRight = offsets + width + 1;

            if (x < width - 1)
            {
                colors[right].a += ale * k7p16;
                colors[right].r += rde * k7p16;
                colors[right].g += gne * k7p16;
                colors[right].b += ble * k7p16;
            }

            if (y < height - 1)
            {
                colors[lower].a += ale * k5p16;
                colors[lower].r += rde * k5p16;
                colors[lower].g += gne * k5p16;
                colors[lower].b += ble * k5p16;

                if (x > 0)
                {
                    colors[lowerLeft].a += ale * k3p16;
                    colors[lowerLeft].r += rde * k3p16;
                    colors[lowerLeft].g += gne * k3p16;
                    colors[lowerLeft].b += ble * k3p16;
                }

                if (x < width - 1)
                {
                    colors[lowerRight].a += ale * k1p16;
                    colors[lowerRight].r += rde * k1p16;
                    colors[lowerRight].g += gne * k1p16;
                    colors[lowerRight].b += ble * k1p16;
                }
            }
            colors[offsets].a = al2;
            colors[offsets].r = rd2;
            colors[offsets].g = gn2;
            colors[offsets].b = bl2;
        }
    }
}
