using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class Dictionary : BaseModel
    {
        [Required]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        [MaxLength(1000)]
        public virtual string Description { get; set; }

        [Required]
        public virtual DateTime Date { get; set; }

        [Required]
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public virtual int FirstLanguageId { get; set; }
        [ForeignKey("FirstLanguageId")]
        public virtual Language FirstLanguage { get; set; }

        [Required]
        public virtual int SecondLanguageId { get; set; }
        [ForeignKey("SecondLanguageId")]
        public virtual Language SecondLanguage { get; set; }

        [NotMapped]
        public override bool IsValid => 
            !string.IsNullOrWhiteSpace(Name) &&
            UserId > 0 && 
            FirstLanguageId > 0 && 
            SecondLanguageId > 0;
    }
}
