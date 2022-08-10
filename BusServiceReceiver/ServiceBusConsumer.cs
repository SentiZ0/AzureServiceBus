using Azure.Messaging.ServiceBus;
using BusServiceReceiver.Features.ProcessData;
using BusServiceReceiver.Models;
using MediatR;

namespace BusServiceReceiver
{
    public interface IServiceBusConsumer
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }

    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private const string QUEUE_NAME = "myqueue";
        private readonly ILogger _logger;
        private ServiceBusProcessor _processor;
        //private readonly IProcessData _processData;
        private readonly IMediator _mediator;

        public ServiceBusConsumer(IConfiguration configuration, ILogger<ServiceBusConsumer> logger, 
          //  IProcessData processData, 
            IMediator mediator)
        {

            _configuration = configuration;
            _logger = logger;

            var connectionString = _configuration.GetConnectionString("BusConnectionString");
            _client = new ServiceBusClient(connectionString);
           // _processData = processData;
            _mediator = mediator;
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(QUEUE_NAME, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var user = args.Message.Body.ToObjectFromJson<UserDTO>();
            var query = new Process(user.Id);
            var result = await _mediator.Send(query);

           // await _processData.Process(user).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
