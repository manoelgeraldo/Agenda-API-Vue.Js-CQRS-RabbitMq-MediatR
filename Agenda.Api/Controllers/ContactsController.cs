using Agenda.Application.Commands;
using Agenda.Application.DTOs;
using Agenda.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ContactsController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<PagedResult<ContactDto>>> Search([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		=> await mediator.Send(new SearchContactsQuery(q, page, pageSize));


		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ContactDto?>> Get(Guid id)
		=> await mediator.Send(new GetContactByIdQuery(id)) is { } dto ? Ok(dto) : NotFound();


		public record UpsertRequest(string Name, string Email, string Phone);


		[HttpPost]
		public async Task<ActionResult<ContactDto>> Create([FromBody] UpsertRequest req)
		{
			var dto = await mediator.Send(new CreateContactCommand(req.Name, req.Email, req.Phone));
			return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
		}


		[HttpPut("{id:guid}")]
		public async Task<ActionResult<ContactDto>> Update(Guid id, [FromBody] UpsertRequest req)
		=> await mediator.Send(new UpdateContactCommand(id, req.Name, req.Email, req.Phone));


		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{ await mediator.Send(new DeleteContactCommand(id)); return NoContent(); }
	}
}
