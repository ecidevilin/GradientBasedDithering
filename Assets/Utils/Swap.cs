using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public static partial class Utils
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}
