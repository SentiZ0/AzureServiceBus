using Azure.Messaging.ServiceBus;
using AzureServiceBus.Models;
using MediatR;
using System.Text.Json;

namespace AzureServiceBus.Features.Command.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly BusDBContext _context;

        private readonly string _serviceConnectionString;

        public CreateUserCommandHandler(BusDBContext context, IConfiguration configuration)
        {
            _context = context;
            _serviceConnectionString = configuration.GetConnectionString("BusConnectionString");
        }

        public async Task<CreateUserCommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            string queueName = "myqueue";

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                Flag = false,
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var client = new ServiceBusClient(_serviceConnectionString);
            var sender = client.CreateSender(queueName);

            var userId = new UserDTO();
            userId.Id = user.Id;
            var body = JsonSerializer.Serialize(userId);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
    
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(body)))
            {
                throw new Exception($"The message is too large to fit in the batch.");
            }

            await sender.SendMessagesAsync(messageBatch);
            await sender.DisposeAsync();
            await client.DisposeAsync();

            return new CreateUserCommandResult();
        }
    }
}
