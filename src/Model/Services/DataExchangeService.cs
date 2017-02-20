using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Model.Models.Database;
using Model.Models.DataExchange;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class DataExchangeService : IDataExchangeService
    {
        private ICsvService CsvService { get; set; }

        private ITranslationsRepository TranslationsRepository { get; set; }

        private IDictionariesRepository DictionariesRepository { get; set; }

        public DataExchangeService(ICsvService csvService, ITranslationsRepository translationsRepository, IDictionariesRepository dictionariesRepository)
        {
            CsvService = csvService;
            TranslationsRepository = translationsRepository;
            DictionariesRepository = dictionariesRepository;
        }


        public bool ImportDictionary(MemoryStream csv, int dictionaryId)
        {
            try
            {
                var list = CsvService.FormatCsv(csv);
                var toAdd = new List<Translation>();
                foreach (TranslationPair translationPair in list)
                {
                    toAdd.Add(new Translation
                    {
                        DictionaryId = dictionaryId,
                        FirstLangWord = translationPair.FirstLanguageWord,
                        SecondLangWord = translationPair.SecondLanguageWord
                    });
                }
                TranslationsRepository.Insert(toAdd);
                TranslationsRepository.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public MemoryStream ExportDictionary(int dictionaryId)
        {
            var dictionary = DictionariesRepository.GetById(dictionaryId);
            var translations = TranslationsRepository.GetTranslationsForDictionary(dictionaryId);
            var translationPairsList = new List<TranslationPair>();
            translationPairsList.Add(new TranslationPair
            {
                FirstLanguageWord = dictionary.FirstLanguage.Name,
                SecondLanguageWord = dictionary.SecondLanguage.Name
            });
            foreach (var translation in translations)
            {
                translationPairsList.Add(new TranslationPair
                {
                    FirstLanguageWord = translation.FirstLangWord,
                    SecondLanguageWord = translation.SecondLangWord
                });
            }
            return CsvService.CreateCsv(translationPairsList);
        }
    }
}
