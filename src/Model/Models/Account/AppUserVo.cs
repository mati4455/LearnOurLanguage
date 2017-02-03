namespace Model.Models.Account
{
    public class AppUserVo
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int AccessLevel { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}