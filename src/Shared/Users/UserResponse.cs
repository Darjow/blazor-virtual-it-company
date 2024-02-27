using Domain.Users;
using Shared.Users;

namespace Shared.Users;

public static class UserResponse
{
    public class AllKlantenIndex
    {
        public List<UserDto.Index> Klanten { get; set; } = new();
        public int Total { get; set; }
    }

    public class DetailKlant: UserDto.KlantDetail  { }

    public class Index: UserDto.Index  { }
    public class Edit
    {
        public int Id { get; set; }
    }

    public class Create: UserDto.Index   {  }

    public class JWTClaims
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public AdminRole? Role { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

    }

    public class AllAdminsIndex
    {
        public List<AdminUserDto.Index> Admins { get; set; } = new();
        public int Total { get; set; }
    }
}