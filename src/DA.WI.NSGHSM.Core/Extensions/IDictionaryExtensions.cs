using System;
using System.Collections.Generic;
using System.Linq;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class IDictionaryExtensions
    {
        public static TValue TryGetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> me, TKey key)
        {
            TValue value = default(TValue);

            if (me == null)
                return value;

            me.TryGetValue(key, out value);

            return value;
        }

        public static string ToKeyValueString<TKey, TValue>(this IDictionary<TKey, TValue> me, string separator = ";")
        {
            if (me == null)
                return null;

            return String.Join(separator, me.Select(_ => $"{_.Key}={_.Value}"));
        }
    }
}
