using System;
using System.Collections.Generic;
using System.IO;
using Model.Models.Database;
using Model.Models.DataExchange;

namespace Model.Services.Interfaces
{
    public interface ICsvService
    {
        MemoryStream CreateCsv(List<TranslationPair> list);

        List<TranslationPair> FormatCsv(MemoryStream file);
    }
}
