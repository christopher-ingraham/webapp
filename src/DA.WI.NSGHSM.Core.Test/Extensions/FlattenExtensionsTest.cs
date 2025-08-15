using DA.WI.NSGHSM.XUnitExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using DA.WI.NSGHSM.Core.Extensions;
using System.Linq;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class FlattenExtensionsTest
    {
        [FactWithAutomaticDisplayName]
        public void FlattenNull_EmptyList()
        {
            object sut = null;
            var result = sut.Flatten();

            Assert.Empty(result);
        }

        [FactWithAutomaticDisplayName]
        public void FlattenInteger()
        {

            var result = 1.Flatten();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(String.Empty, result.First().Key);
            Assert.Equal(1, result.First().Value);
        }

        [FactWithAutomaticDisplayName]
        public void FlattenString()
        {
            var result = "Test".Flatten();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(String.Empty, result.First().Key);
            Assert.Equal("Test", result.First().Value);
        }


        [FactWithAutomaticDisplayName]
        public void FlattenDate()
        {
            var dateTime = DateTime.Now;
            var result = dateTime.Flatten();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(String.Empty, result.First().Key);
            Assert.Equal(dateTime, result.First().Value);
        }



        [FactWithAutomaticDisplayName]
        public void FlattenObject()
        {
            var test = new Test { Id = 1, Name = "Test", Qty = 10.0, SubTest = null };
            var result = test.Flatten();

            Assert.NotNull(result);
            Assert.Equal(4, result.Length);

            Assert.Equal("Id", result[0].Key);
            Assert.Equal(1, result[0].Value);

            Assert.Equal("Name", result[1].Key);
            Assert.Equal("Test", result[1].Value);

            Assert.Equal("Qty", result[2].Key);
            Assert.Equal(10.0, result[2].Value);

            Assert.Equal("SubTest", result[3].Key);
            Assert.Null(result[3].Value);
        }



        [FactWithAutomaticDisplayName]
        public void FlattenAnonymous()
        {
            var test = new { Id = 1, Name = "Test", Qty = 10.0 };
            var result = test.Flatten();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);

            Assert.Equal("Id", result[0].Key);
            Assert.Equal(1, result[0].Value);

            Assert.Equal("Name", result[1].Key);
            Assert.Equal("Test", result[1].Value);

            Assert.Equal("Qty", result[2].Key);
            Assert.Equal(10.0, result[2].Value);
        }


        [FactWithAutomaticDisplayName]
        public void FlattenAnonymousWithSubObject()
        {
            var test = new Test { Id = 1, Name = "Test", Qty = 10.0, SubTest = new SubTest { Message = "SubTest" } };
            var result = test.Flatten();

            Assert.NotNull(result);
            Assert.Equal(4, result.Length);

            var i = 0;
            Assert.Equal("Id", result[i].Key);
            Assert.Equal(1, result[i].Value);

            i++;
            Assert.Equal("Name", result[i].Key);
            Assert.Equal("Test", result[i].Value);

            i++;
            Assert.Equal("Qty", result[i].Key);
            Assert.Equal(10.0, result[i].Value);

            i++;
            Assert.Equal("SubTest.Message", result[i].Key);
            Assert.Equal("SubTest", result[i].Value);
        }

        [FactWithAutomaticDisplayName]
        public void FlattenArrayOfObjects()
        {
            var test = new Test[]
            {
                new Test { Id = 1, Name = "Test", Qty = 10.0, SubTest = new SubTest { Message = "SubTest" } },
                new Test { Id = 2, Name = "Test1" }
            };
            var result = test.Flatten();

            Assert.NotNull(result);
            Assert.Equal(8, result.Length);

            var i = 0;
            Assert.Equal("[0].Id", result[i].Key);
            Assert.Equal(1, result[i].Value);

            i++;
            Assert.Equal("[0].Name", result[i].Key);
            Assert.Equal("Test", result[i].Value);

            i++;
            Assert.Equal("[0].Qty", result[i].Key);
            Assert.Equal(10.0, result[i].Value);

            i++;
            Assert.Equal("[0].SubTest.Message", result[i].Key);
            Assert.Equal("SubTest", result[i].Value);

            i++;
            Assert.Equal("[1].Id", result[i].Key);
            Assert.Equal(2, result[i].Value);

            i++;
            Assert.Equal("[1].Name", result[i].Key);
            Assert.Equal("Test1", result[i].Value);

            i++;
            Assert.Equal("[1].Qty", result[i].Key);
            Assert.Null(result[i].Value);

            i++;
            Assert.Equal("[1].SubTest", result[i].Key);
            Assert.Null(result[i].Value);
        }

        [FactWithAutomaticDisplayName]
        public void FlattenDictionary()
        {
            var test = new Dictionary<string, object>
            {
                { "Key1",  1 },
                { "Key2",  2 },
            };

            var result = test.Flatten();

            var i = 0;
            Assert.Equal("[Key1]", result[i].Key);
            Assert.Equal(1, result[i].Value);

            i++;
            Assert.Equal("[Key2]", result[i].Key);
            Assert.Equal(2, result[i].Value);
        }


        class Test
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public double? Qty { get; set; }

            public SubTest SubTest { get; set; }
        }

        class SubTest
        {
            public string Message { get; set; }
        }

        class TestItem
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public double? Qty { get; set; }
        }
    }
}
