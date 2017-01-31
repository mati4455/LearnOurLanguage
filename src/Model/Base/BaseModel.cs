using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Base
{
    public abstract class BaseModel
    {
        [Key]
        public virtual int Id { get; set; }

        [NotMapped]
        public abstract bool IsValid { get; }
    }
}