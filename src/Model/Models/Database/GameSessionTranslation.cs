using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class GameSessionTranslation : BaseModel
    {
        [Required]
        public virtual int GameSessionId { get; set; }
        public virtual GameSession GameSession { get; set; }

        [Required]
        public virtual int TranslationId { get; set; }
        public virtual Translation Translation { get; set; }

        [Required]
        public virtual bool Correct { get; set; }

        public virtual int Duration { get; set; }

        [NotMapped]
        public override bool IsValid => GameSessionId > 0 && TranslationId > 0;
    }
}
