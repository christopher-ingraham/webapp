using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.XUnitExtensions;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class IDictionaryExtensionsTest
    {

        #region TryGetValueOrDefault

        [FactWithAutomaticDisplayName]
        public void TryGetValueOrDefault_DictionaryNull_KeyNull()
        {
            Dictionary<string, string> sut = null;

            var result = sut.TryGetValueOrDefault(null);

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void TryGetValueOrDefault_DictionaryEmpty_KeyNull_Throws_ArgumentNullException()
        {
            var sut = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException> (() => sut.TryGetValueOrDefault(null));
        }

        [FactWithAutomaticDisplayName]
        public void TryGetValueOrDefault_DictionaryWithKeyAValue1_KeyA_Excpecting_Value1()
        {
            var sut = new Dictionary<string, string>  { { "A", "1" } };

            var result = sut.TryGetValueOrDefault("A");

            Assert.Equal("1", result);
        }


        [FactWithAutomaticDisplayName]
        public void TryGetValueOrDefault_DictionaryWithKeyAValue1_KeyB_Excpecting_Null()
        {
            var sut = new Dictionary<string, string> { { "A", "1" } };

            var result = sut.TryGetValueOrDefault("B");

            Assert.Null(result);
        }

        #endregion

        #region ToKeyValueString

        [FactWithAutomaticDisplayName]
        public void ToKeyValueString_Null_Returns_Null()
        {
            Dictionary<string, string> sut = null;

            var result = sut.ToKeyValueString();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ToKeyValueString_Empty_Returns_Empty()
        {
            var sut = new Dictionary<string, string>();

            var result = sut.ToKeyValueString();

            Assert.Equal(String.Empty, result);
        }

        [FactWithAutomaticDisplayName]
        public void ToKeyValueString_A1B2_Returns_AEq1_BEq2()
        {
            var sut = new Dictionary<string, object> { { "A", 1 }, { "B", 2 } };

            var result = sut.ToKeyValueString();

            Assert.Equal("A=1;B=2", result);
        }

        #endregion
    }
}
