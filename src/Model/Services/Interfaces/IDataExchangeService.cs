using System.IO;

namespace Model.Services.Interfaces
{
    public interface IDataExchangeService
    {
        bool ImportDictionary(MemoryStream csv, int dictionaryId);

        MemoryStream ExportDictionary(int dictionaryId);
    }
}
