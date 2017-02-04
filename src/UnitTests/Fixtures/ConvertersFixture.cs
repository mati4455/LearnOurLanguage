using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Helpers;
using Xunit;

namespace UnitTests.Fixtures
{
    public class ConvertersFixture
    {
        [Fact]
        public void StringToBytesConvertTest()
        {
            var test = "learnourlanguage";
            var bytes = ConvertersHelper.GetBytes(test);

            Assert.Equal(bytes[0], 108);
            Assert.Equal(bytes[2], 101);
            Assert.Equal(bytes[4], 97);
            Assert.Equal(bytes[6], 114);
            Assert.Equal(bytes[8], 110);
            Assert.Equal(bytes[10], 111);
            Assert.Equal(bytes[12], 117);
            Assert.Equal(bytes[14], 114);
            Assert.Equal(bytes[16], 108);
            Assert.Equal(bytes[18], 97);
            Assert.Equal(bytes[20], 110);
            Assert.Equal(bytes[22], 103);
            Assert.Equal(bytes[24], 117);
            Assert.Equal(bytes[26], 97);
            Assert.Equal(bytes[28], 103);
            Assert.Equal(bytes[30], 101);
        }

        [Fact]
        public void BytesToStringConverter()
        {
            var expected = "WeAreTheChampions";

            var bytes = new byte[]
            {
                87, 0, 101, 0, 65, 0, 114, 0, 101, 0, 84, 0, 104, 0, 101, 0, 67,
                0, 104, 0, 97, 0, 109, 0, 112, 0, 105, 0, 111, 0, 110, 0, 115, 0
            };
            var result = ConvertersHelper.GetString(bytes);
            Assert.Equal(result, expected);
        }
    }
}
