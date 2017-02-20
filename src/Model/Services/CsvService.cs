using System;
using System.Collections.Generic;
using System.IO;
using Model.Models.Database;
using Model.Models.DataExchange;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class CsvService : ICsvService
    {
        public MemoryStream CreateCsv(List<TranslationPair> list)
        {
            var output = new MemoryStream();
            if (list.Count == 0)
            {
                return output;
            }
            using (var writer = new StreamWriter(output))
            { 
                foreach (var translation in list)
                {
                    writer.WriteLine($"{translation.FirstLanguageWord};{translation.SecondLanguageWord}");
                }
            }
            return output;
        }

        public List<TranslationPair> FormatCsv(MemoryStream file)
        {
            List<TranslationPair> list = new List<TranslationPair>();

            using (var streamReader = new StreamReader(file))
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
