using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Core.Helpers;
using DA.WI.NSGHSM.XUnitExtensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class IEnumerableExtensionsTest
    {
        #region Join

        [FactWithAutomaticDisplayName]
        public void Join_Null_Returns_Null()
        {
            IEnumerable<string> sut = null;

            var result = sut.Join();

            Assert.Null(result);
        }

        [FactWithAutomaticDisplayName]
        public void Join_StringArray_Returns_CsvString()
        {
            var sut = new[] { "one", "two", "three" };

            var result = sut.Join();

            Assert.Equal("one,two,three", result);
        }

        [FactWithAutomaticDisplayName]
        public void Join_StringArray_With_PipeSeparator_Returns_PipeSeparatedValuesString()
        {
            var sut = new[] { "one", "two", "three" };

            var result = sut.Join("|");

            Assert.Equal("one|two|three", result);
        }

        #endregion

        #region ToCsv

        private enum TestEnum
        {
            Value1 = 0,
            Value2,
            Value3
        }

        [FactWithAutomaticDisplayName]
        public void ToCsv_EnumArray_Returns_SeparatedEnumNamesString()
        {
            var sut = new TestEnum[] { TestEnum.Value1, TestEnum.Value2, TestEnum.Value3 };

            var result = sut.ToCsv();

            Assert.Equal("Value1,Value2,Value3", result);
        }

        #endregion

        #region SplitInChuncks

        [FactWithAutomaticDisplayName]
        public void SplitInChunks_EmptyArray()
        {
            var sut = new int[] { };

            var result = sut.SplitInChunks();

            Assert.Empty(result);
        }


        [FactWithAutomaticDisplayName]
        public void SplitInChunks_ArrayOf100_OneChunk()
        {
            var sut = Enumerable.Range(1, 100);

            var result = sut.SplitInChunks();

            Assert.Single(result);
            Assert.Equal(sut.Count(), result.First().Count());
        }

        [FactWithAutomaticDisplayName]
        public void SplitInChunks_ArrayOf101_TwoChunks()
        {
            var sut = Enumerable.Range(1, 101);

            var result = sut.SplitInChunks();

            Assert.Equal(2, result.Count());
            Assert.Equal(100, result.First().Count());
            Assert.Single(result.Last());
        }

        #endregion

        #region DistinctByTakeFirst

        private class PermissionDto
        {
            public string Name { get; set; }

            public PermissionLevel Level { get; set; }
        }

        private enum PermissionLevel
        {
            NoAccess = 0,
            Read = 1,
            ReadWrite = 2
        }

        [FactWithAutomaticDisplayName]
        public void DistinctByTakeFirst_FilterDistinctPermissionsWithHigherLevel()
        {
            var permissions = InitHelper.Array<PermissionDto>(
                _ => { _.Name = "A"; _.Level = PermissionLevel.NoAccess; },
                _ => { _.Name = "A"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "B"; _.Level = PermissionLevel.Read; },
                _ => { _.Name = "B"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "B"; _.Level = PermissionLevel.Read; },
                _ => { _.Name = "C"; _.Level = PermissionLevel.Read; },
                _ => { _.Name = "C"; _.Level = PermissionLevel.NoAccess; },
                _ => { _.Name = "C"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "C"; _.Level = PermissionLevel.NoAccess; },
                _ => { _.Name = "D"; _.Level = PermissionLevel.Read; }
            );

            var result = permissions.DistinctByTakeFirst(_ => _.Name, _ => _.Level, isDescending: true);

            var expected = InitHelper.Array<PermissionDto>(
                _ => { _.Name = "A"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "B"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "C"; _.Level = PermissionLevel.ReadWrite; },
                _ => { _.Name = "D"; _.Level = PermissionLevel.Read; }
            );

            result.Should().BeEquivalentTo(expected);
        }


        #endregion

    }
}
