using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class Game : BaseModel
    {
        [Required]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        [NotMapped]
        public override bool IsValid => !string.IsNullOrWhiteSpace(Name);
    }
}
