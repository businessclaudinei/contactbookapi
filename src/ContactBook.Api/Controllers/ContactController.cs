using System.Threading.Tasks;
using AutoMapper;
using ContactBook.Commands.AddContact;
using ContactBook.Commands.UpdateContact;
using ContactBook.Domain.Commands.AddUser;
using ContactBook.Domain.Commands.RemoveContact;
using ContactBook.Domain.Commands.UpdateUser;
using ContactBook.Infrastructure.Data.Query.Queries.GetContacts;
using ContactBook.Infrastructure.Data.Query.Queries.GetUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Api.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCacheService;

        public ContactController (IMediator mediator, IMapper mapper, IResponseCacheService responseCacheService) {
            _mediator = mediator;
            _mapper = mapper;
            _responseCacheService = responseCacheService;
        }

        // GET api/values/5
        [HttpGet]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<GetContactsQueryResponse>> Get (string token) {
            return await _mediator.Send (new GetContactsQuery { Token = token });
        }

        // POST api/values
        [HttpPost]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<AddContactCommandResponse>> Post ([FromBody] AddContactCommand command, string token) {
            command.Token = token;
            return await _mediator.Send (command);
        }

        // PUT api/values/5
        [HttpPut ("{email}")]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<UpdateContactCommandResponse>> Put ([FromBody] UpdateContactCommand command, string token) {
            command.Token = token;
            return await _mediator.Send (command);
        }

        // DELETE api/values/5
        [HttpDelete ("{email}")]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<RemoveContactCommandResponse>> Delete (string email, string token) {
            return await _mediator.Send (new RemoveContactCommand { Token = token, Email = email });
        }

        // POST api/values
        [HttpPost ("account")]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<AddUserCommandResponse>> CreateUser ([FromBody] AddUserCommand command) {
            return await _mediator.Send (command);
        }

        [HttpPost ("account/login")]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<GetUserQueryResponse>> GetToken ([FromBody] GetUserQuery query) {
            return await _mediator.Send (query);
        }

        [HttpPut ("account")]
        [EnableCors ("AllowOrigin")]
        public async Task<ActionResult<UpdateUserCommandResponse>> UpdatePassword ([FromBody] UpdateUserCommand command, string token) {
            command.Token = token;
            return await _mediator.Send (command);
        }
    }
}