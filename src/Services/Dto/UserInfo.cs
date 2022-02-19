using Enum;

namespace Services.Dto
{
    public class UserInfo
    {
        public string Mail { get; set; }

        public string Name { get; set; }

        public RoleEnum Role { get; set; }

        public string RoleName { get; set; }
    }
}