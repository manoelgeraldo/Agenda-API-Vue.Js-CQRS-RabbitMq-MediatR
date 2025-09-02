using Agenda.Application.Commands;
using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using AutoMapper;

namespace Agenda.Application.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Contact, ContactDto>();
		}
	}
}
