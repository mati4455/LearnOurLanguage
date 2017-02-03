using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Model.Base;
using System;

namespace Model.Models.Database
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string Login { get; set; }

        [Required]
        [MaxLength(256)]
        public virtual string Email { get; set; }
        
        [Required]
        [MaxLength(512)]
        public virtual string Password { get; set; }

        [Required]
        [Column(TypeName = "varbinary(16)")]
        public virtual byte[] Salt { get; set; }

        [Required]
        public virtual int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual DateTime Date { get; set; }

        [NotMapped]
        public virtual string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public override bool IsValid
        {
            get
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                return regex.Match(Email).Success &&
                       (RoleId > 0) &&
                       !string.IsNullOrWhiteSpace(FirstName) &&
                       !string.IsNullOrWhiteSpace(LastName);
            }
        }
    }
}
