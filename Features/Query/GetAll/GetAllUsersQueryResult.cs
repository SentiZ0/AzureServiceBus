using MediatR;

namespace AzureServiceBus.Features.Query.GetAll
{
    public class GetAllUsersQueryResult
    {
        public List<UserDTO> Users { get; set; }

        public class UserDTO
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public bool Flag { get; set; }
        }
    }
}
