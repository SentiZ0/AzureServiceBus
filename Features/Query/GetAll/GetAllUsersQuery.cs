using MediatR;

namespace AzureServiceBus.Features.Query.GetAll
{
    public class GetAllUsersQuery : IRequest<GetAllUsersQueryResult>
    {
    }
}
