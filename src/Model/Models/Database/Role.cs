using Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Models.Database
{
    public class Role : BaseModel
    {
        [Required]
        [MaxLength(64)]
        public virtual string Name { get; set; }

        [Required]
        public virtual int AccessLevel { get; set; }

        [NotMapped]
        public override bool IsValid
        {
            get { return true; }
        }
    }
}