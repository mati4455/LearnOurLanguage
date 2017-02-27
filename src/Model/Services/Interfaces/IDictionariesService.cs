using Model.Models.Dictionaries;

namespace Model.Services.Interfaces
{
    public interface IDictionariesService
    {
        int InsertOrUpdate(DictionaryDTO dictionary);
        bool Delete(int dictionaryId);
        int CopyDictionary(int dictionaryId, int userId);
        bool UpdateDictionary(int dictionaryId);
    }
}
