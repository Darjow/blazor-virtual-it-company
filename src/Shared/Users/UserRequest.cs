namespace Shared.Users;

public static class UserRequest
{
    public class DetailKlant
    {
        public int Id { get; set; }
    }

    public class Create: UserDto.Create
    {
    }

    public class Edit: UserDto.EditCustomer
    {
    }
}