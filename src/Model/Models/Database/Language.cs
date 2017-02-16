using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class Language : BaseModel
    {
        [Required]
        [MaxLength(30)]
        public virtual string Name { get; set; }

        [Required]
        [MaxLength(6)]
        public virtual string Code { get; set; }

        [NotMapped]
        public override bool IsValid => !string.IsNullOrWhiteSpace(Name);
    }
}
