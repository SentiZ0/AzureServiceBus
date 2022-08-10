using BusServiceReceiver.Features.ProcessData;
using BusServiceReceiver.Models;
using BusServiceReceiver.Validation;
using MediatR;

namespace BusServiceReceiver
{
    public class ProcessData : IProcessData
    {
        private IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProcessData(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task Process(UserDTO userId)
        {
            var query = new Process(userId.Id);

            var result = await _mediator.Send(query);
        }
    }
}
