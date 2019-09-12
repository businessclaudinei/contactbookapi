using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContactBook.Infrastructure.Data.Query.Queries.Get;
using MediatR;
using AutoMapper;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;

namespace ContactBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
	private readonly IResponseCacheService _responseCacheService;

         public ValuesController(IMediator mediator, IMapper mapper, IResponseCacheService responseCacheService)
         {
            _mediator = mediator;
            _mapper = mapper;
            _responseCacheService = responseCacheService;
         }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("/get")]
        public async Task<ActionResult<Response>> Get([FromQuery]Query query)
        {
            return await _mediator.Send(query);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
