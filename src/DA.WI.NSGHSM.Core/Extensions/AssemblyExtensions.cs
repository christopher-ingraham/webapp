using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class AssemblyExtensions
    {

        public static string GetCopyright(this Assembly me)
        {
            if (me == null)
                return string.Empty;

            Assembly targetAssembly = Assembly.GetEntryAssembly();

            return  targetAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

        }

        public static string GetCompany(this Assembly me)
        {
            if (me == null)
                return string.Empty;

            Assembly targetAssembly = Assembly.GetEntryAssembly();

            return targetAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;

        }

        public static string GetDescription(this Assembly me)
        {
            if (me == null)
                return string.Empty;

            Assembly targetAssembly = Assembly.GetEntryAssembly();

            return targetAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        }

        public static string GetVersion(this Assembly me)
        {
            if (me == null)
                return string.Empty;

            Assembly targetAssembly = Assembly.GetEntryAssembly();

            return targetAssembly.GetName().Version.ToString();

        }


    }
}
