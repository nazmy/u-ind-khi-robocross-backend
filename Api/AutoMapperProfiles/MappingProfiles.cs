using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.AutoMapperProfiles
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Client, ClientResponse>().ReverseMap();
			CreateMap<Client, CreateClientInputDto>().ReverseMap();
			CreateMap<Client, UpdateClientInputDto>().ReverseMap();

			CreateMap<Compound, CompoundResponse>().ReverseMap();
			CreateMap<Compound, CreateCompoundInputDto>().ReverseMap();
			CreateMap<Compound, UpdateCompoundInputDto>().ReverseMap();
		}
	}
}

