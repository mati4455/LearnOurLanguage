using Model.Helpers;
using Xunit;

namespace UnitTests.Fixtures
{
    public class AuthFixture
    {
        [Theory]
        [InlineData("RbXaile82Z!@", new byte[] { 98, 68, 153, 201, 78, 248, 119, 143, 57, 207, 46, 40, 90, 188, 251, 122 }, "LV63Q2woT4Lr2sTrPUEB9VTZNK5nMj4pZwt4vXaAJ+c=")]
        [InlineData("Haselko14##", new byte[] { 222, 94, 161, 38, 62, 203, 43, 79, 204, 90, 118, 198, 101, 54, 192, 115 }, "zOytoKv9/cfYbVaHdxfQsPRPEnhvDwDija90cd1zIwk=")]
        [InlineData("Y6sfvzGGS72#", new byte[] { 5, 89, 232, 56, 59, 40, 70, 184, 121, 83, 181, 27, 122, 245, 178, 243 }, "BgLu2LAQK31cYwEiRRYTbmx2GJFremhY93l+egcIjqY=")]
        public void HashGeneratorCheck(string password, byte[] salt, string hash)
        {
            var hashCheck = HashHelper.ComputeHash(password, salt);
            Assert.Equal(hash, hashCheck);
        }
    }
}
