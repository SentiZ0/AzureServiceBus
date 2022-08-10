using AzureServiceBus.Models;
using MediatR;

namespace AzureServiceBus.Features.Query.GetAll
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersQueryResult>
    {
        private readonly BusDBContext _context;

        public GetAllUsersQueryHandler(BusDBContext context)
        {
            _context = context;
        }

        public async Task<GetAllUsersQueryResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _context.Users.Select(x => new GetAllUsersQueryResult.UserDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age,
                Flag = x.Flag,
            }).ToList();

            return new GetAllUsersQueryResult()
            { Users = users };



        }
    }
}
