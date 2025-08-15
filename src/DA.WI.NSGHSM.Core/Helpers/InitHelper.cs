using System;
using System.Collections.Generic;
using System.Linq;

namespace DA.WI.NSGHSM.Core.Helpers
{
    public class InitHelper
    {
        public static T[] Array<T>(params Action<T>[] setters)
            where T : new()
        {
            return setters.Select(setter =>
            {
                var item = new T();
                setter(item);

                return item;
            })
            .ToArray();
        }


        public static string[] EndPointsBaseURLRelpacer(string[] enpointsForReplacing, string[] targetURLToReplace, string placeHolder)
        {
            if (enpointsForReplacing != null)
            {
                List<string> targetReplacedUrl = new List<string>();

                for (int i = 0; i < enpointsForReplacing.Length; i++)
                {
                    for (int j = 0; j < targetURLToReplace.Length; j++)
                    {

                        //if (targetURLToReplace.Length > i)
                        //{
                        //    targetReplacedUrl.Add(targetURLToReplace[i]);
                        //}
                        //else
                        //{
                        //targetReplacedUrl.Add(targetURLToReplace[targetURLToReplace.Length - 1]);
                        //}
                        targetReplacedUrl.Add(targetURLToReplace[j]);
                        targetReplacedUrl[targetReplacedUrl.Count - 1] = targetReplacedUrl[targetReplacedUrl.Count - 1].Replace(placeHolder, enpointsForReplacing[i]);
                    }


                }
                return targetReplacedUrl.ToArray();
            }
            return targetURLToReplace;
        }


        public static string EndPointsBaseURLRelpacer(string enpointsForReplacing, string targetURLToReplace, string placeHolder)
        {
            if (enpointsForReplacing != null)
            {
                return targetURLToReplace.Replace(placeHolder, enpointsForReplacing);
            }
            return targetURLToReplace;
        }

    }

}
