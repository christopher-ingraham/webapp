using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.XUnitExtensions;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class ListExtensionsTest
    {
        [FactWithAutomaticDisplayName]
        public void ReverseImmutable_NullList_Returns_Null()
        {
            List<string> sut = null;

            var result = sut.ReverseImmutable();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void ReverseImmutable_EmptyList_Returns_EmptyList()
        {
            var sut = new List<string>();

            var result = sut.ReverseImmutable();

            Assert.Empty(result);
        }

        [FactWithAutomaticDisplayName]
        public void ReverseImmutable_ABC_Returns_CBA()
        {
            var sut = new List<string>() { "A", "B", "C" };

            var result = sut.ReverseImmutable();

            Assert.Equal("C", result[0]);
            Assert.Equal("B", result[1]);
            Assert.Equal("A", result[2]);
        }
    }
}
