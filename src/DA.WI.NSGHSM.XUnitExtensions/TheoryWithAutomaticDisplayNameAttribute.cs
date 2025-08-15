using System.Runtime.CompilerServices;

namespace DA.WI.NSGHSM.XUnitExtensions
{
    public class TheoryWithAutomaticDisplayNameAttribute : Xunit.TheoryAttribute
    {
        public TheoryWithAutomaticDisplayNameAttribute(string charsToReplace = "_",
                                                     string replacementChars = " ",
                                                     [CallerMemberName] string testMethodName = "")
        {
            if (charsToReplace != null)
            {
                base.DisplayName = testMethodName?.Replace(charsToReplace, replacementChars);
            }
        }
    }
}
