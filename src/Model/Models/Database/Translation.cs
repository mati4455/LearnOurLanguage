using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class Translation : BaseModel
    {
        [Required]
        public virtual int DictionaryId { get; set; }
        public virtual Dictionary Dictionary { get; set; }

        [Required]
        [MaxLength(512)]
        public virtual string FirstLangWord { get; set; }

        [Required]
        [MaxLength(512)]
        public virtual string SecondLangWord { get; set; }

        [NotMapped]
        public override bool IsValid =>
            DictionaryId > 0 &&
            !string.IsNullOrWhiteSpace(FirstLangWord) &&
            !string.IsNullOrWhiteSpace(SecondLangWord);

        public override bool Equals(object obj)
        {
            var tr = (Translation) obj;
            return tr.FirstLangWord.ToUpper().Equals(FirstLangWord.ToUpper()) &&
                   tr.SecondLangWord.ToUpper().Equals(SecondLangWord.ToUpper());
        }

        public override int GetHashCode()
        {
            return FirstLangWord.ToUpper().GetHashCode() ^ SecondLangWord.ToUpper().GetHashCode();
        }
    }
}
