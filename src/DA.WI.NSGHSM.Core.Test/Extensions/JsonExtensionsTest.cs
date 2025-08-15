using DA.WI.NSGHSM.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DA.WI.NSGHSM.Core.Test.Extensions
{
    public class JsonExtensionsTest
    {
        [Fact]
        public void CloneNull()
        {
            object sut = null;

            var result = sut.JsonClone();

            Assert.Null(result);
        }

        [Fact]
        public void CloneInt()
        {
            var sut = 1;

            var result = sut.JsonClone();

            Assert.Equal(1, result);
        }

        [Fact]
        public void CloneString()
        {
            var sut = "test";

            var result = sut.JsonClone();

            Assert.Equal("test", result);
        }

        [Fact]
        public void CloneList()
        {
            var sut = new List<int> { 1, 2, 3 };

            var result = sut.JsonClone();

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void CloneDynamic()
        {
            var sut = new { val1 = "test", val2 = 1 };

            var result = sut.JsonClone();

            Assert.Equal("test", result.val1);
            Assert.Equal(1, result.val2);
        }
        
        [Fact]
        public void CloneClass()
        {
            var sut = new JsonCloneTestClass { val1 = "test", val2 = 1 };

            var result = sut.JsonClone();

            Assert.Equal("test", result.val1);
            Assert.Equal(1, result.val2);
        }
        
        [Fact]
        public void CloneArrayOfClassesWithPolymorfism()
        {
            var sut = new JsonCloneTestClassBase[]
            {
                new JsonCloneTestClassSpec1 { baseVal = "BASE1", spec1Val= "SPEC1" },
                new JsonCloneTestClassSpec2 { baseVal = "BASE2", spec2Val= "SPEC2" }
            };

            var result = sut.JsonClone();

            Assert.Equal(2, result.Count());

            Assert.IsType<JsonCloneTestClassSpec1>(result[0]);
            Assert.Equal("BASE1", result[0].baseVal);
            Assert.Equal("SPEC1", ((JsonCloneTestClassSpec1)result[0]).spec1Val);

            Assert.IsType<JsonCloneTestClassSpec2>(result[1]);
            Assert.Equal("BASE2", result[1].baseVal);
            Assert.Equal("SPEC2", ((JsonCloneTestClassSpec2)result[1]).spec2Val);
        }

        public class JsonCloneTestClass
        {
            public string val1 { get; set; }

            public int val2 { get; set; }
        }

        public class JsonCloneTestClassBase
        {
            public string baseVal { get; set; }
        }

        public class JsonCloneTestClassSpec1 : JsonCloneTestClassBase
        {
            public string spec1Val { get; set; }
        }

        public class JsonCloneTestClassSpec2 : JsonCloneTestClassBase
        {
            public string spec2Val { get; set; }
        }

        public class JsonCloneTestClassArray
        {
            public object val1 { get; set; }
        }

        [Fact]
        public void ToJsonNull()
        {
            object sut = null;

            var result = sut.ToJson();

            Assert.Null(result);
        }

        [Fact]
        public void ToJsonInt()
        {
            var sut = 1;

            var result = sut.ToJson();

            Assert.Equal("1", result);
        }

        [Fact]
        public void ToJsonString()
        {
            var sut = "test";

            var result = sut.ToJson();

            Assert.Equal("\"test\"", result);
        }

        [Fact]
        public void ToJsonList()
        {
            var sut = new List<int> { 1, 2, 3 };

            var result = sut.ToJson();

            Assert.Equal("[1,2,3]", result);
        }

        [Fact]
        public void ToJsonDynamic()
        {
            var sut = new { val1 = "test", val2 = 1 };

            var result = sut.ToJson();

            Assert.Equal("{\"val1\":\"test\",\"val2\":1}", result);
        }
        
        [Fact]
        public void ToJsonClass()
        {
            var sut = new ToJsonTestClass { val1 = "test", val2 = 1 };

            var result = sut.ToJson();

            Assert.Equal("{\"val1\":\"test\",\"val2\":1}", result);
        }

        public class ToJsonTestClass
        {
            public string val1 { get; set; }

            public int val2 { get; set; }
        }

        [Fact]
        public void FromJsonNull()
        {
            string sut = null;

            var result = sut.FromJson<object>();

            Assert.Null(result);
        }

        [Fact]
        public void FromJsonInt()
        {
            var sut = "1";

            var result = sut.FromJson<int>();

            Assert.Equal(1, result);
        }

        [Fact]
        public void FromJsonString()
        {
            var sut = "\"test\"";

            var result = sut.FromJson<string>();

            Assert.Equal("test", result);
        }

        [Fact]
        public void FromJsonList()
        {
            var sut = "[1,2,3]";

            var result = sut.FromJson<List<int>>();

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }
        
        [Fact]
        public void FromJsonClass()
        {
            var sut = "{\"val1\":\"test\",\"val2\":1}";

            var result = sut.FromJson<FromJsonTestClass>();

            Assert.Equal("test", result.val1);
            Assert.Equal(1, result.val2);
        }

        public class FromJsonTestClass
        {
            public string val1 { get; set; }

            public int val2 { get; set; }
        }
    }
}
