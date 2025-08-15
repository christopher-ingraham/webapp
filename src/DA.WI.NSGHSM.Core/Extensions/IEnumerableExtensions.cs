using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
        
        public static string Join<T>(this IEnumerable<T> me, string separator = ",")
        {
            if (me == null)
                return null;

            return String.Join(separator, me);
        }

        public static string ToCsv<T>(this IEnumerable<T> me,
                                      string separator = ",",
                                      bool shouldEscapeSeparators = false,
                                      Func<T, string> toString = null)
        {
            if (me == null)
                return null;

            var stringElements = me
                .Select(_ =>
                {
                    if (_ == null)
                        return null;

                    var valueString = toString?.Invoke(_) ?? _.ToString();

                    if (shouldEscapeSeparators == true)
                        valueString = valueString.Replace(separator, $"{separator}{separator}");

                    return valueString;
                });

            return stringElements.Join(separator);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> me)
        {
            if (me == null)
                return true;

            return (me.Any() == false);
        }

        public static T[] ToArrayOrEmpty<T>(this IEnumerable<T> me)
        {
            if (me == null)
                return new T[] { };

            return me.ToArray();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)) == true)
                {
                    yield return element;
                }
            }
        }

        public static T PickOneOrDefault<T>(this IEnumerable<T> me)
        {
            if ((me == null) || (me.Any() == false))
                return default(T);

            var rnd = new Random();

            int index = rnd.Next(0, me.Count());

            return me.ElementAt(index);
        }

        public static IEnumerable<IEnumerable<T>> SplitInChunks<T>(this IEnumerable<T> me, int chunkSize = 100)
        {
            if (me == null)
                return null;

            var chunks = me
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            return chunks;
        }

        public static IEnumerable<TSource> DistinctByTakeFirst<TSource, TDistinctByKey, TOrderByKey>(
            this IEnumerable<TSource> me, 
            Func<TSource, TDistinctByKey> distinctBySelector, 
            Func<TSource, TOrderByKey> orderBySelector, 
            bool isDescending = false)
        {
            if (me == null)
                return null;

            var grouped = me.GroupBy(distinctBySelector);

            var orderedGroups = isDescending
                ? grouped.Select(_ => _.OrderByDescending(orderBySelector))
                : grouped.Select(_ => _.OrderBy(orderBySelector));

            var firstGroupItems = orderedGroups.Select(_ => _.First());

            return firstGroupItems;
        }
    }
}
