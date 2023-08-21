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
			CreateMap<Client, ClientOutputDto>().ReverseMap();
			CreateMap<Client, CreateClientInputDto>().ReverseMap();
			CreateMap<Client, UpdateClientInputDto>().ReverseMap();
		}
	}
}

