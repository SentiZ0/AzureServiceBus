using MediatR;

namespace AzureServiceBus.Features.Command.Create
{
    public class CreateUserCommand : IRequest<CreateUserCommandResult>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
