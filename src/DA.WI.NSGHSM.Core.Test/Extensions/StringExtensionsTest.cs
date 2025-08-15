using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.XUnitExtensions;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class StringExtensionsTest
    {
        #region IsEmpty

        [FactWithAutomaticDisplayName]
        public void IsEmpty_NullReturnsFalse()
        {
            string sut = null;

            var result = sut.IsEmpty();

            Assert.False(result);
        }

        [FactWithAutomaticDisplayName]
        public void IsEmpty_EmptyReturnsTrue()
        {
            string sut = String.Empty;

            var result = sut.IsEmpty();

            Assert.True(result);
        }

        [FactWithAutomaticDisplayName]
        public void IsEmpty_NotEmptyReturnsFalse()
        {
            string sut = "abc";

            var result = sut.IsEmpty();

            Assert.False(result);
        }

        #endregion

        #region IsNullOrEmpty

        [FactWithAutomaticDisplayName]
        public void IsNullOrEmpty_NullReturnsTrue()
        {
            string sut = null;

            var result = sut.IsNullOrEmpty();

            Assert.True(result);
        }

        [FactWithAutomaticDisplayName]
        public void IsNullOrEmpty_EmptyReturnsTrue()
        {
            string sut = String.Empty;

            var result = sut.IsNullOrEmpty();

            Assert.True(result);
        }

        [FactWithAutomaticDisplayName]
        public void IsNullOrEmpty_NotEmptyReturnsFalse()
        {
            string sut = "abc";

            var result = sut.IsNullOrEmpty();

            Assert.False(result);
        }

        #endregion

        #region ToStringWithNullOrEmptyDescription

        [FactWithAutomaticDisplayName]
        public void ToStringWithNullOrEmptyDescription_EmptyReturnsEmptyDescription()
        {
            string sut = String.Empty;

            var result = sut.ToStringWithNullOrEmptyDescription();

            Assert.Equal("<empty>", result);
        }

        [FactWithAutomaticDisplayName]
        public void ToStringWithNullOrEmptyDescription_NullReturnsNullDescription()
        {
            string sut = null;

            var result = sut.ToStringWithNullOrEmptyDescription();

            Assert.Equal("<null>", result);

        }


        [FactWithAutomaticDisplayName]
        public void ToStringWithNullOrEmptyDescription_NotEmptyReturnsStringDescription()
        {
            string sut = "abc";

            var result = sut.ToStringWithNullOrEmptyDescription();

            Assert.Equal("abc", result);
        }

        #endregion

        #region Capitalize

        [FactWithAutomaticDisplayName]
        public void Capitalize_Null_Returns_Null()
        {
            string sut = null;

            var result = sut.Capitalize();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void Capitalize_Empty_Returns_Empty()
        {
            string sut = String.Empty;

            var result = sut.Capitalize();

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void Capitalize_a_Returns_A()
        {
            string sut = "a";

            var result = sut.Capitalize();

            Assert.Equal("A", result);
        }

        [FactWithAutomaticDisplayName]
        public void Capitalize_1_Returns_1()
        {
            string sut = "1";

            var result = sut.Capitalize();

            Assert.Equal("1", result);
        }

        [FactWithAutomaticDisplayName]
        public void Capitalize_abc_Returns_Abc()
        {
            string sut = "abc";

            var result = sut.Capitalize();

            Assert.Equal("Abc", result);
        }

        #endregion

        #region Trunc

        [FactWithAutomaticDisplayName]
        public void Trunc_Null_Returns_Null()
        {
            string sut = null;

            var result = sut.Trunc(10);

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void Trunc_Empty_Returns_Empty()
        {
            string sut = String.Empty;

            var result = sut.Trunc(10);

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void Trunc_0_a_Returns_Empty()
        {
            string sut = "a";

            var result = sut.Trunc(0);

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void Trunc_1_a_Returns_a()
        {
            string sut = "a";

            var result = sut.Trunc(0);

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void Trunc_10_a_Returns_a()
        {
            string sut = "a";

            var result = sut.Trunc(10);

            Assert.Equal("a", result);
        }


        [FactWithAutomaticDisplayName]
        public void Trunc_1_abc_Returns_a()
        {
            string sut = "abc";

            var result = sut.Trunc(1);

            Assert.Equal("a", result);
        }

        #endregion

        #region RemoveMultipleTabsAndSpaces

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Null_Returns_Null()
        {
            string sut = null;

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Empty_Returns_Empty()
        {
            string sut = String.Empty;

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_A_Returns_A()
        {
            string sut = "A";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Aspace_Returns_Aspace()
        {
            string sut = "A ";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A ", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Atab_Returns_Atab()
        {
            string sut = "A\t";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A ", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Atabspace_Returns_Aspace()
        {
            string sut = "A \t";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A ", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Aspacetab_Returns_Aspace()
        {
            string sut = "A \t";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A ", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_Aspacetabnewline_Returns_Aspacenewline()
        {
            string sut = "A \t\n";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal("A \n", result);
        }

        [FactWithAutomaticDisplayName]
        public void RemoveMultipleTabsAndSpaces_spacetabnewlineAspace_Returns_spacenewlineAspace()
        {
            string sut = " \t\nA ";

            var result = sut.RemoveMultipleTabsAndSpaces();

            Assert.Equal(" \nA ", result);
        }

        #endregion
    }
}
