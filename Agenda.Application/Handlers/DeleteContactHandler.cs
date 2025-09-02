using Agenda.Application.Commands;
using Agenda.Domain.Contracts;
using MediatR;

namespace Agenda.Application.Handlers
{
	public class DeleteContactHandler(IContactRepository repo) : IRequestHandler<DeleteContactCommand, Unit>
	{
		public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
		{
			var existing = await repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Contato não encontrado.");
			existing.SoftDelete();
			await repo.UpdateAsync(existing);
			return Unit.Value;
		}
	}
}
