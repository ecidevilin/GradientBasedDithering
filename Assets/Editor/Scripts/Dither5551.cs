using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    partial class Dithering
    {
        public static void Dither5551(Color[] colors, int offsets, int width, int height, int x, int y)
        {
            Color32 c = colors[offsets];
            byte r = (byte)(c.r & k5551Mask);
            byte g = (byte)(c.g & k5551Mask);
            byte b = (byte)(c.b & k5551Mask);

            var re = (c.r - r) / k8BitFloat;
            var ge = (c.g - g) / k8BitFloat;
            var be = (c.b - b) / k8BitFloat;

            if (x < width - 1)
            {
                int right = offsets + 1;
                colors[right].r += re * k7p16;
                colors[right].g += ge * k7p16;
                colors[right].b += be * k7p16;
            }

            if (y < height - 1)
            {
                var lower = offsets + width;
                colors[lower].r += re * k5p16;
                colors[lower].g += ge * k5p16;
                colors[lower].b += be * k5p16;

                if (x > 0)
                {
                    var lowerLeft = offsets + width - 1;
                    colors[lowerLeft].r += re * k3p16;
                    colors[lowerLeft].g += ge * k3p16;
                    colors[lowerLeft].b += be * k3p16;
                }

                if (x < width - 1)
                {

                    var lowerRight = offsets + width + 1;
                    colors[lowerRight].r += re * k1p16;
                    colors[lowerRight].g += ge * k1p16;
                    colors[lowerRight].b += be * k1p16;
                }
            }

            colors[offsets] = new Color32(r, g, b, 255);
        }

    }
}
