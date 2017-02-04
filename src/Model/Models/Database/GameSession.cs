using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models.Database
{
    public class GameSession : BaseModel
    {
        [Required]
        public virtual int GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public virtual DateTime DateStart { get; set; }

        public virtual DateTime? DateEnd { get; set; }

        [NotMapped]
        public override bool IsValid => GameId > 0 && UserId > 0;
    }
}
