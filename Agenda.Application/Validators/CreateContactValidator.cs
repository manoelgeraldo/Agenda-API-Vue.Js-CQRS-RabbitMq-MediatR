using Agenda.Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Application.Validators
{
	public class CreateContactValidator : AbstractValidator<CreateContactCommand>
	{
		public CreateContactValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Phone).NotEmpty().MinimumLength(10).MaximumLength(20);
		}
	}
}
