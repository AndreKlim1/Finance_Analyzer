using System.Data;
using UsersService.Models.Enums;

namespace UsersService.Models
{
    public class User : BaseModel<long>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Role Role { get; set; }
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
