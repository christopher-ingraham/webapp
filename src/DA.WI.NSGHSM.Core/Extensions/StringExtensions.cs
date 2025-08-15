using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string me)
        {
            return me == String.Empty;
        }

        public static bool IsNullOrEmpty(this string me)
        {
            return String.IsNullOrEmpty(me);
        }

        public static string ToStringWithNullOrEmptyDescription(this string me,
                                                                string nullDescription = "<null>",
                                                                string emptyDescription = "<empty>")
        {
            if (me == null)
                return nullDescription;

            if (me.IsEmpty() == true)
                return emptyDescription;

            return me;
        }

        public static string Capitalize(this string me)
        {
            if ((me == null) || (me.IsEmpty() == true))
                return me;

            return $"{me.First().ToString().ToUpper()}{me.Substring(1)}";
        }

        public static string Trunc(this string me, int maxChars)
        {
            if (me == null)
                return null;

            if (me.Length < maxChars)
                return me;

            return me.Substring(0, maxChars);
        }

        public static string RemoveMultipleTabsAndSpaces(this string me)
        {
            if (me.IsNullOrEmpty() == true)
                return me;

            return Regex.Replace(me, @"[ \t]+", " ");
        }

    }
}
