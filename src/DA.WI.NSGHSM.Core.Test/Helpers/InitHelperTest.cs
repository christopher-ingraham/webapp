using DA.WI.NSGHSM.Core.Helpers;
using DA.WI.NSGHSM.XUnitExtensions;
using FluentAssertions;

namespace DA.WI.NSGHSM.Core.Test.Helpers
{
    public class InitHelperTest
    {
        [FactWithAutomaticDisplayName]
        public void Init_Array_With_SomeDto()
        {
            var result = InitHelper.Array<SomeDto>(
                _ => { _.IntProp = 1; _.StringProp = "A"; },
                _ => { _.IntProp = 2; _.StringProp = "B"; }
            );

            var expected = new SomeDto[]
            {
                new SomeDto() { IntProp = 1, StringProp = "A" },
                new SomeDto() { IntProp = 2, StringProp = "B" }
            };

            result.Should().BeEquivalentTo(expected);
        }

        private class SomeDto
        {
            public int IntProp { get; set; }

            public string StringProp { get; set; }
        }
    }
}
