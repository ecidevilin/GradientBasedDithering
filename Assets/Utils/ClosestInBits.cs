using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public static partial class Utils
    {
        public static int ClosestInBits(int val, byte bit)
        {
            return (val >> bit << bit);
        }
    }
}
