using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models
{
    public class Category : BaseModel<long>
    {
        public long? UserId { get; set; }
        public string CategoryName { get; set; }
        public CategoryType CategoryType { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }
}
