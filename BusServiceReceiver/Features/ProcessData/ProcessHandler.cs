using BusServiceReceiver.Models;
using BusServiceReceiver.Validation;
using MediatR;

namespace BusServiceReceiver.Features.ProcessData
{
    public class ProcessHandler : IRequestHandler<Process, ProcessResult>
    {
       // private readonly BusDBContext _context;
        private readonly IServiceProvider _provider;
        public ProcessHandler(IServiceProvider provider)
        {
            _provider = provider;
            //_context = context;
        }

        public async Task<ProcessResult> Handle(Process request, CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BusDBContext>();
            var user = context.Users.FirstOrDefault(x => x.Id == request.Id);

            if (user != null)
            {
                var validator = new UserValidator();

                var result = validator.Validate(user);

                if (result.IsValid)
                {
                    user.Flag = true;
                }

                context.SaveChanges();
            }

            return new ProcessResult();
        }
    }
}
