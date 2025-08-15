using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class FlattenExtensions
    {
        public static KeyValuePair<string, object>[] Flatten(this object me)
        {
            var result = new List<KeyValuePair<string, object>>();

            me.Flatten(result);

            return result.ToArray();
        }

        private static void Flatten(this object me, List<KeyValuePair<string, object>> result, string prefix = null)
        {
            if (IsNullRootObject(me, prefix))
                return;

            if (IsNotNullNotStringObject(me))
            {
                var propertyPrefix = (prefix != null) ? $"{prefix}." : String.Empty;

                // dynamic / expando objects
                if (IsDynamicExpandoObject(me) == true)
                {
                    FlattenDynamicExpandoObject(me, result, propertyPrefix);
                    return;
                }

                if (IsDictionary(me) == true)
                {
                    FlattenDictionary(me, result, prefix);
                    return;
                }

                if (IsEnumerable(me) == true)
                {
                    FlattenEnumerable(me, result, prefix);
                    return;
                }

                var type = me.GetType();
                if (type.IsClass == true)
                {
                    FlattenClass(me, result, propertyPrefix, type);
                    return;
                }
            }

            result.Add(new KeyValuePair<string, object>(prefix ?? String.Empty, me));
        }

        private static bool IsNotNullNotStringObject(object me)
        {
            var isString = me is string;
            return ((me != null) && (isString == false));
        }

        private static bool IsNullRootObject(object me, string prefix)
        {
            return (me == null) && (prefix == null);
        }

        private static bool IsComplexObject(object me)
        {
            return ((me != null) && ((me is string) == false));
        }

        private static void FlattenClass(object me, List<KeyValuePair<string, object>> result, string propertyPrefix, Type type)
        {
            foreach (var pi in type.GetProperties())
            {
                var value = pi.GetValue(me);
                value.Flatten(result, $"{propertyPrefix}{pi.Name}");
            }
        }

        private static void FlattenEnumerable(object me, List<KeyValuePair<string, object>> result, string prefix)
        {
            int index = 0;
            foreach (var item in (me as IEnumerable))
            {
                item.Flatten(result, $"{prefix}[{index++}]");
            }
        }

        private static bool IsEnumerable(object me)
        {
            return ((me is IEnumerable) == true);
        }

        private static void FlattenDictionary(object me, List<KeyValuePair<string, object>> result, string prefix)
        {
            foreach (DictionaryEntry item in (me as IDictionary))
            {
                item.Value.Flatten(result, $"{prefix}[{item.Key}]");
            }
        }

        private static bool IsDictionary(object me)
        {
            return ((me is IDictionary) == true);
        }

        private static void FlattenDynamicExpandoObject(object me, List<KeyValuePair<string, object>> result, string propertyPrefix)
        {
            foreach (var item in (me as IDictionary<string, object>))
            {
                item.Value.Flatten(result, $"{propertyPrefix}{item.Key}");
            }
        }

        private static bool IsDynamicExpandoObject(object me)
        {
            return ((me is System.Dynamic.IDynamicMetaObjectProvider) == true)
                && ((me is IDictionary<string, object>) == true);
        }
    }
}
