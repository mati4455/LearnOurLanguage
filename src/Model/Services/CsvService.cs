using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Model.Models.Database;
using Model.Models.DataExchange;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class CsvService : ICsvService
    {
        public MemoryStream CreateCsv(List<TranslationPair> list)
        {
            if (list.Count == 0)
            {
                return new MemoryStream();
            }
            var stringBuilder = new StringBuilder();
            foreach (var translation in list)
            {
                stringBuilder.AppendLine($"{translation.FirstLanguageWord};{translation.SecondLanguageWord}");
            }
            var encodedString = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return new MemoryStream(encodedString);
        }

        public List<TranslationPair> FormatCsv(MemoryStream file)
        {
            List<TranslationPair> list = new List<TranslationPair>();
            file.Position = 0;

            using (var streamReader = new StreamReader(file, Encoding.UTF8))
            {
                var headers = streamReader.ReadLine();
                while (!streamReader.EndOfStream)
                {
                    var  translationPairs= streamReader.ReadLine();
                    var splitted = translationPairs.Split(';');
                    list.Add(new TranslationPair
                    {
                        FirstLanguageWord = splitted[0],
                        SecondLanguageWord = splitted[1]
                    });
                }
            }

            return list;
        }
    }
}
