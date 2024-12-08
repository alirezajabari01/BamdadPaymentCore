using System.ComponentModel.DataAnnotations;

namespace BamdadPaymentCore.Domain.Entites
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserFamily { get; set; }
        public string? UserMobile { get; set; }
        public string? UserUserName { get; set; }
        public string? UserPass { get; set; }
        public bool? UserIsActive { get; set; }
        public DateTime? UserCreateDate { get; set; }
    }

}
