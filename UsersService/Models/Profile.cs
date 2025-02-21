namespace UsersService.Models
{
    public class Profile : BaseModel<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
