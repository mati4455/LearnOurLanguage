using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Model.Models.Database;
using Model.Models.Dictionaries;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class DictionariesService : IDictionariesService
    {
        private DatabaseContext Context { get; }
        private IDictionariesRepository DictionariesRepository { get; }
        private ITranslationsRepository TranslationsRepository { get; }
        private IGameSessionsRepository GameSessionsRepository { get; }
        private IGameSessionTranslationsRepository GameSessionTranslationsRepository { get; }

        public DictionariesService(
            DatabaseContext context,
            ITranslationsRepository translationsRepository,
            IDictionariesRepository dictionariesRepository,
            IGameSessionsRepository gameSessionsRepository,
            IGameSessionTranslationsRepository gameSessionTranslationsRepository)
        {
            Context = context;
            TranslationsRepository = translationsRepository;
            DictionariesRepository = dictionariesRepository;
            GameSessionsRepository = gameSessionsRepository;
            GameSessionTranslationsRepository = gameSessionTranslationsRepository;
        }

        public int InsertOrUpdate(DictionaryDTO dictionaryVo)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var dictionary = Mapper.Map<Dictionary>(dictionaryVo);

                    if (dictionary.Id > 0)
                    {
                        Context.Entry(dictionary).State = EntityState.Modified;
                        DictionariesRepository.Update(dictionary);
                    }
                    else {
                        dictionary.Date = DateTime.Now;
                        dictionary.ParentDictionaryId = null;
                        DictionariesRepository.Insert(dictionary);
                    }
                    DictionariesRepository.Save();

                    dictionaryVo.TranslationList.ToList().ForEach(x => x.DictionaryId = dictionary.Id);

                    var translations = dictionaryVo.TranslationList.Select(x => x.Id).ToList();
                    var currentTranslations = TranslationsRepository
                        .GetAll()
                        .Where(x => x.DictionaryId == dictionary.Id)
                        .ToList();
                    var ids = currentTranslations.Select(x => x.Id).ToList();
                    var toDelete = currentTranslations.Where(x => !translations.Contains(x.Id)).ToList();
                    var toAdd = dictionaryVo.TranslationList?.Where(x => !ids.Contains(x.Id)).ToList() ?? new List<Translation>();
                    var toUpdate = dictionaryVo.TranslationList?.Where(x => ids.Contains(x.Id)).ToList() ?? new List<Translation>();

                    foreach (var trans in toDelete)
                    {
                        GameSessionTranslationsRepository.DeleteByTranslationId(trans.Id);
                        TranslationsRepository.Delete(trans);
                    }

                    foreach (var trans in toUpdate)
                        TranslationsRepository.Update(trans);

                    foreach (var trans in toAdd)
                        TranslationsRepository.Insert(trans);

                    TranslationsRepository.Save();

                    transaction.Commit();
                    return dictionary.Id;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return -1;
                }
            }
        }

        public bool Delete(int dictionaryId)
        {
            TranslationsRepository.DeleteByDictionaryId(dictionaryId);
            TranslationsRepository.Save();

            GameSessionTranslationsRepository.DeleteByDictionaryId(dictionaryId);
            GameSessionTranslationsRepository.Save();

            GameSessionsRepository.DeleteByDictionaryId(dictionaryId);
            GameSessionsRepository.Save();

            DictionariesRepository.Delete(dictionaryId);
            DictionariesRepository.Save();

            return true;
        }

        public int CopyDictionary(int dictionaryId, int userId)
        {
            var translations = TranslationsRepository.GetTranslationsForDictionary(dictionaryId).ToList();
            var dictionary = DictionariesRepository.GetById(dictionaryId);

            Context.Entry(dictionary).State = EntityState.Detached;

            dictionary.Id = 0;
            dictionary.UserId = userId;
            dictionary.ParentDictionaryId = dictionaryId;
            dictionary.User = null;
            dictionary.Date = DateTime.Today;
            dictionary.IsPublic = false;
            DictionariesRepository.Insert(dictionary);
            DictionariesRepository.Save();

            translations.ForEach(translation =>
            {
                Context.Entry(translation).State = EntityState.Detached;
                translation.Id = 0;
                translation.DictionaryId = dictionary.Id;
                translation.Dictionary = null;
            });

            var check = TranslationsRepository.Insert(translations) && TranslationsRepository.Save();

            return check ? dictionary.Id : -1;
        }

        public bool UpdateDictionary(int dictionaryId)
        {
            var dictionary = DictionariesRepository.GetById(dictionaryId);
            if (dictionary.ParentDictionaryId == null) return false;

            var currentTranslations = TranslationsRepository.GetTranslationsForDictionary(dictionaryId).ToList();
            var parentTranslations = TranslationsRepository.GetTranslationsForDictionary(dictionary.ParentDictionaryId.Value).ToList();
            var toAdd = parentTranslations.Except(currentTranslations).ToList();

            toAdd.ForEach(trans =>
            {
                var translation = new Translation
                {
                    DictionaryId = dictionaryId,
                    FirstLangWord = trans.FirstLangWord,
                    SecondLangWord = trans.SecondLangWord
                };
                TranslationsRepository.Insert(translation);
            });

            return TranslationsRepository.Save(); ;
        }
    }
}
