using Azure.Messaging.ServiceBus;
using AzureServiceBus.Models;
using MediatR;

namespace AzureServiceBus.Features.Command.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResult>
    {
        private readonly BusDBContext _context;

        private readonly string _serviceConnectionString;

        public UpdateUserCommandHandler(BusDBContext context, IConfiguration configuration)
        {
            _context = context;
            _serviceConnectionString = configuration.GetConnectionString("BusConnectionString");
        }

        public async Task<UpdateUserCommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            string queueName = "myqueue";

            var user = _context.Users.FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
            {
                return null;
            }

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Age = request.Age;

            _context.SaveChanges();

            var client = new ServiceBusClient(_serviceConnectionString);
            var sender = client.CreateSender(queueName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Updated Id: {user.Id}")))
            {
                throw new Exception($"The message is too large to fit in the batch.");
            }

            await sender.SendMessagesAsync(messageBatch);
            await sender.DisposeAsync();
            await client.DisposeAsync();

            return new UpdateUserCommandResult();
        }
    }
}
