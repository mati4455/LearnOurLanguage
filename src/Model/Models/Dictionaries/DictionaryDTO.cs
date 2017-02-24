using Model.Models.Database;
using System;
using System.Collections.Generic;

namespace Model.Models.Dictionaries
{
    public class DictionaryDTO
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Public { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual int FirstLanguageId { get; set; }
        public virtual int SecondLanguageId { get; set; }
        public virtual IList<Translation> TranslationList { get; set; }
    }
}

