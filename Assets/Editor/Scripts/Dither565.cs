using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Chaos
{
    partial class Dithering
    {


        public static Dictionary<float, float> cache5 = new Dictionary<float, float>(256);
        public static Dictionary<float, float> cache6 = new Dictionary<float, float>(256);

        static Color ClosetColor(Color c)
        {
            c.r = cache5.ContainsKey(c.r) ? cache5[c.r] : (cache5[c.r] = Mathf.Clamp01(Mathf.Floor(c.r * 32) / 31));
            c.g = cache6.ContainsKey(c.g) ? cache6[c.g] : (cache6[c.g] = Mathf.Clamp01(Mathf.Floor(c.g * 64) / 63));
            c.b = cache5.ContainsKey(c.b) ? cache5[c.b] : (cache5[c.b] = Mathf.Clamp01(Mathf.Floor(c.b * 32) / 31));
            return c;
        }


        public static void _Dither565(Color[] colors, int offsets, int width, int height, int x, int y)
        {
            float rd = colors[offsets].r;
            float gn = colors[offsets].g;
            float bl = colors[offsets].b;

            var rd2 = Mathf.Clamp01(Mathf.Floor(rd * skr) * k1p31);
            var gn2 = Mathf.Clamp01(Mathf.Floor(gn * skg) * k1p63);
            var bl2 = Mathf.Clamp01(Mathf.Floor(bl * skr) * k1p31);

            var rde = rd - rd2;
            var gne = gn - gn2;
            var ble = bl - bl2;

            var right = offsets + 1;
            var lowerLeft = offsets + width - 1;
            var lower = offsets + width;
            var lowerRight = offsets + width + 1;

            if (x < width - 1)
            {
                colors[right].r += rde * k15p32;
                colors[right].g += gne * k29p64;
                colors[right].b += ble * k15p32;
            }

            if (y < height - 1)
            {
                colors[lower].r += rde * k11p32;
                colors[lower].g += gne * k21p64;
                colors[lower].b += ble * k11p32;

                if (x > 0)
                {
                    colors[lowerLeft].r += rde * k5p32;
                    colors[lowerLeft].g += gne * k11p64;
                    colors[lowerLeft].b += ble * k5p32;
                }

                if (x < width - 1)
                {
                    colors[lowerRight].r += rde * k1p32;
                    colors[lowerRight].g += gne * k3p64;
                    colors[lowerRight].b += ble * k1p32;
                }
            }

            colors[offsets].r = rd2;
            colors[offsets].g = gn2;
            colors[offsets].b = bl2;
        }

        //在Dither4444的基础上提供另一种优化方案，RGB565、RGB565+A8
        public static void Dither565(Texture2D texture)
        {
            var width = texture.width;
            var height = texture.height;

            var colors = texture.GetPixels();
            var offsets = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Dither565(colors, offsets, width, height, x, y);
                    offsets++;
                }
            }

            texture.SetPixels(colors);
            EditorUtility.CompressTexture(texture, TextureFormat.RGB565, UnityEditor.TextureCompressionQuality.Best);
        }


        public static void Dither565(Color[] colors, int offsets, int width, int height, int x, int y)
        {
            float rd = colors[offsets].r;
            float gn = colors[offsets].g;
            float bl = colors[offsets].b;

            Color col = ClosetColor(colors[offsets]);
            var rd2 = col.r;
            var gn2 = col.g;
            var bl2 = col.b;

            var rde = rd - rd2;
            var gne = gn - gn2;
            var ble = bl - bl2;

            var right = offsets + 1;
            var lowerLeft = offsets + width - 1;
            var lower = offsets + width;
            var lowerRight = offsets + width + 1;

            if (x < width - 1)
            {
                colors[right].r += rde * k15p32;
                colors[right].g += gne * k29p64;
                colors[right].b += ble * k15p32;
            }

            if (y < height - 1)
            {
                colors[lower].r += rde * k11p32;
                colors[lower].g += gne * k21p64;
                colors[lower].b += ble * k11p32;

                if (x > 0)
                {
                    colors[lowerLeft].r += rde * k5p32;
                    colors[lowerLeft].g += gne * k11p64;
                    colors[lowerLeft].b += ble * k5p32;
                }

                if (x < width - 1)
                {
                    colors[lowerRight].r += rde * k1p32;
                    colors[lowerRight].g += gne * k3p64;
                    colors[lowerRight].b += ble * k1p32;
                }
            }

            colors[offsets].r = rd2;
            colors[offsets].g = gn2;
            colors[offsets].b = bl2;
        }

    }
}
