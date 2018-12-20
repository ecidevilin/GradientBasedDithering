using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chaos
{
    partial class Dithering
    {

        static readonly float k1p31 = 1.0f / 31.0f;
        static readonly float k1p32 = 1.0f / 32.0f;
        static readonly float k5p32 = 5.0f / 32.0f;
        static readonly float k11p32 = 11.0f / 32.0f;
        static readonly float k15p32 = 15.0f / 32.0f;
        static readonly float k1p63 = 1.0f / 63.0f;
        static readonly float k3p64 = 3.0f / 64.0f;
        static readonly float k11p64 = 11.0f / 64.0f;
        static readonly float k21p64 = 21.0f / 64.0f;
        static readonly float k29p64 = 29.0f / 64.0f;


        static readonly float k1p15 = 1.0f / 15.0f;
        static readonly float k1p16 = 1.0f / 16.0f;
        static readonly float k3p16 = 3.0f / 16.0f;
        static readonly float k5p16 = 5.0f / 16.0f;
        static readonly float k7p16 = 7.0f / 16.0f;

        static readonly float skr = 32; //R&B压缩到5位，所以取2的5次方
        static readonly float skg = 64; //G压缩到6位，所以取2的6次方

        static readonly float k1b = 1.0f;
        static readonly float k3b = 3.0f;
        static readonly float k5b = 5.0f;
        static readonly float k7b = 7.0f;
        static readonly float perB = 16.0f;
        static readonly int bitsPerB = 4;

        static readonly int k5551Mask = 248;
        static readonly float k8BitFloat = 256.0f;

    }
}
