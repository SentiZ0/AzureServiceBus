using AzureServiceBus.Features.Command.Create;
using AzureServiceBus.Features.Command.Update;
using AzureServiceBus.Features.Query.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateUserCommand command)
        {
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var response = await _mediator.Send(new GetAllUsersQuery());

            return Ok(response);
        }
    }
}
