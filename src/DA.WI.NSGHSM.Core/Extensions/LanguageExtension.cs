using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static DA.WI.NSGHSM.Core.App.Configuration.ApplicationConfig;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class LanguageExtension
    {
        private const string regExpression = @"^[a-z]{2}-[A-Z]{2}$";
        public static string GetLanguageCode(this string me)
        {
            if (me == null)
                return string.Empty;

            return me.Split('-')[0].Trim();

        }

        public static string GetCountryCode(this string me)
        {
            if (me == null)
                return string.Empty;

            return me.Split('-')[1].Trim();

        }

        public static bool IsCodeValid(this string me)
        {
            bool res = false;
            if (me.IsNullOrEmpty() == false)
            {
                try
                {
                    Regex regex = new Regex(regExpression);
                    Match match = regex.Match(me, 0, 5);
                    res = match.Success;
                }
                catch(ArgumentNullException) { }
                catch (ArgumentOutOfRangeException) { }
                catch (RegexMatchTimeoutException) { }
            }              
            return res;
        }
    }

    
}
