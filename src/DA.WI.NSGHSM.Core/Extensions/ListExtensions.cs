using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ReverseImmutable<T>(this List<T> me)
        {
            if (me == null)
                return me;

            // Array linq Reverse is immutable, instead of List<T>.Reverse
            return me
                .ToArray()
                .Reverse()
                .ToList();
        }

    }
}
