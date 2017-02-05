using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class UserDictionary : BaseModel
    {
        [Required]
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public virtual int DictionaryId { get; set; }
        public virtual Dictionary Dictionary { get; set; }

        [NotMapped]
        public override bool IsValid => UserId > 0 && DictionaryId > 0;
    }
}
