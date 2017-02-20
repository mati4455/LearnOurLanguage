using System.Collections.Generic;
using System.IO;
using Model.Helpers;
using Model.Models.Database;
using Model.Services;
using Xunit;

namespace UnitTests.Fixtures
{

    public class CsvFixture
    {
        private const string Text =
            @"L1;L2
test;test1
test2;test3
test4;test5";

        [Fact]
        public void TestFormattingCsv()
        {
            var csvService = new CsvService();
            var ms = StreamHelper.GenerateStreamFromString(Text);
            var list = csvService.FormatCsv(ms);

            Assert.Equal("test", list[0].FirstLanguageWord);
            Assert.Equal(3, list.Count);
        }

        //[Fact]
        //public void TestCreatingCsv()
        //{
        //    var csvService = new CsvService();
        //    var list = new List<Translation>()
        //    {
        //        new Translation()
        //        {
        //            FirstLangWord = "test",
        //            SecondLangWord = "test1"
        //        },
        //        new Translation()
        //        {
        //            FirstLangWord = "test2",
        //            SecondLangWord = "test3"
        //        },
        //        new Translation()
        //        {
        //            FirstLangWord = "test4",
        //            SecondLangWord = "test5"
        //        }
        //    };
        //    MemoryStream a =csvService.CreateCsv(list);

        //}
    }
}