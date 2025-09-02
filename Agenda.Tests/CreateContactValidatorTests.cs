using Agenda.Application.Commands;
using Agenda.Application.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Agenda.Tests
{
	public class CreateContactValidatorTests
	{
		[Fact]
		public void Should_Have_Error_When_Fields_Are_Invalid()
		{
			var v = new CreateContactValidator();
			var r = v.TestValidate(new CreateContactCommand("", "bad", "123"));
			r.ShouldHaveValidationErrorFor(x => x.Name);
			r.ShouldHaveValidationErrorFor(x => x.Email);
			r.ShouldHaveValidationErrorFor(x => x.Phone);
		}
	}
}
