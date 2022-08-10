using MediatR;

namespace BusServiceReceiver.Features.ProcessData
{
    public class Process : IRequest<ProcessResult>
    {
        public int Id { get; set; }

        public Process(int id)
        {
            Id = id;
        }
    }
}
