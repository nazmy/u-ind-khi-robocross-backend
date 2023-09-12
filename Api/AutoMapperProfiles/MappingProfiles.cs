using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace khi_robocross_api.AutoMapperProfiles
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Client, ClientResponse>().ReverseMap();
			CreateMap<Client, CreateClientInputDto>().ReverseMap();
			CreateMap<Client, UpdateClientInputDto>().ReverseMap();
			CreateMap<Point, GeoJsonPoint<GeoJson3DCoordinates>>().ReverseMap();
			
			CreateMap<Building, BuildingResponse>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom( (compound, response) => 
						response.Coordinates = new Point(
							new Position(compound.Coordinates.Coordinates.X,compound.Coordinates.Coordinates.Y,compound.Coordinates.Coordinates.Z)) ))
				.ReverseMap();
			
			CreateMap<CreateBuildingInputDto, Building>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				.ReverseMap();
			
			CreateMap<UpdateBuildingInputDto, Building>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				.ReverseMap();

			CreateMap<Compound, CompoundResponse>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom( (compound, response) => 
						response.Coordinates = new Point(
							new Position(compound.Coordinates.Coordinates.X,compound.Coordinates.Coordinates.Y,compound.Coordinates.Coordinates.Z)) ))
				.ReverseMap();
			
			CreateMap<CreateCompoundInputDto, Compound>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				 .ReverseMap();
			
			CreateMap<UpdateCompoundInputDto, Compound>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				.ReverseMap();
			
			CreateMap<Line, LineResponse>().ReverseMap();
			CreateMap<CreateLineInputDto, Line>().ReverseMap();
			CreateMap<UpdateLineInputDto, Line>().ReverseMap();
			
			CreateMap<Unit, UnitResponse>().ReverseMap();
			CreateMap<Unit, CreateUnitInput>().ReverseMap();
			CreateMap<Unit, UpdateUnitInputDto>().ReverseMap();
			
			CreateMap<SceneObject, SceneObjectResponse>().ReverseMap();
			CreateMap<SceneObject, CreateSceneObjectInput>().ReverseMap();
			CreateMap<SceneObject, UpdateSceneObjectInput>().ReverseMap();
			
			CreateMap<Robot, RobotResponse>().ReverseMap();
			CreateMap<Robot, CreateRobotInput>().ReverseMap();
			CreateMap<Robot, UpdateRobotInput>().ReverseMap();
		}
	}
}

