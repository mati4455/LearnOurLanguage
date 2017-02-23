using Model.Models.Dictionaries;

namespace Model.Services.Interfaces
{
    public interface IDictionariesService
    {
        bool InsertOrUpdate(DictionaryDTO dictionary);
        bool Delete(int dictionaryId);
        bool CopyDictionary(int dictionaryId, int userId);
        bool UpdateDictionary(int dictionaryId);
    }
}
