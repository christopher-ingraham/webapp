using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DA.WI.NSGHSM.XUnitExtensions
{
    /// <summary>
    ///     Attribute that is applied to a method to indicate that it is a fact that
    ///     should be run by the test runner. The name of the method will be used as
    ///     the test method's <see cref="Xunit.FactAttribute.DisplayName"/> after
    ///     being reformatted by replacing specific characters in the method's name
    ///     with other characters.
    ///     
    ///     reference: https://bitwiseguy.wordpress.com/2015/11/23/creating-readable-xunit-test-method-names-automatically/
    /// </summary>
    public class FactWithAutomaticDisplayNameAttribute : Xunit.FactAttribute
    {
        /// <summary>
        ///     Attribute that is applied to a method to indicate that it is a fact that
        ///     should be run by the test runner. The name of the method will be used as
        ///     the test method's <see cref="Xunit.FactAttribute.DisplayName"/> after
        ///     being reformatted by replacing specific characters in the method's name
        ///     with other characters.
        /// </summary>
        /// <param name="charsToReplace">
        ///     A <see cref="string"/> containing the characters
        ///     to replace in the test method's name (e.g. "_").
        /// </param>
        /// <param name="replacementChars">
        ///     A <see cref="string"/> containing the characters (e.g. " ") that will
        ///     replace those specified by the <paramref name="charsToReplace"/> parameter
        ///     that are found in the test method's name.
        /// </param>
        /// <param name="testMethodName">
        ///     This is automatically set to the name of the current method;
        ///     there's no need to set a value for this parameter.
        /// </param>
        public FactWithAutomaticDisplayNameAttribute(string charsToReplace = "_",
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
