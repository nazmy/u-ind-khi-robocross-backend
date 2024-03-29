﻿using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;
using domain.Repositories;
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
			CreateMap<Client, CreateClientInput>().ReverseMap();
			CreateMap<Client, UpdateClientInput>().ReverseMap();
			CreateMap<Point, GeoJsonPoint<GeoJson3DCoordinates>>().ReverseMap();
			
			CreateMap<Building, BuildingResponse>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom( (compound, response) => 
						response.Coordinates = new Point(
							new Position(compound.Coordinates.Coordinates.X,compound.Coordinates.Coordinates.Y,compound.Coordinates.Coordinates.Z)) ))
				.ReverseMap();
			
			CreateMap<CreateBuildingInput, Building>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				.ReverseMap();
			
			CreateMap<UpdateBuildingInput, Building>()
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
			
			CreateMap<CreateCompoundInput, Compound>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				 .ReverseMap();
			
			CreateMap<UpdateCompoundInput, Compound>()
				.ForMember(destinationMember => destinationMember.Coordinates,
					sourceMember => sourceMember.MapFrom((dto, compound) => 
						compound.Coordinates = new GeoJsonPoint<GeoJson3DCoordinates>(new GeoJson3DCoordinates(dto.Coordinates.Coordinates.Latitude,dto.Coordinates.Coordinates.Longitude,dto.Coordinates.Coordinates.Altitude.GetValueOrDefault()))))
				.ReverseMap();
			
			CreateMap<Line, LineResponse>().ReverseMap();
			CreateMap<CreateLineInput, Line>().ReverseMap();
			CreateMap<UpdateLineInput, Line>().ReverseMap();
			
			CreateMap<Unit, UnitResponse>().ReverseMap();
			CreateMap<Unit, CreateUnitInput>().ReverseMap();
			CreateMap<Unit, UpdateUnitInput>().ReverseMap();
			
			CreateMap<SceneObject, SceneObjectResponse>().ReverseMap();
			CreateMap<SceneObject, CreateSceneObjectInput>().ReverseMap();
			CreateMap<SceneObject, UpdateSceneObjectInput>().ReverseMap();
			
			CreateMap<AppRole, RoleResponse>();

			CreateMap<AppUser, UserResponse>().ForMember(
					destinationMember => destinationMember.RoleId,
					sourceMember =>
						sourceMember.MapFrom((user, response) => response.RoleId = user.Roles.First().ToString()))
				.ForMember(
					destinationMember => destinationMember.CreatedAt,
					sourceMember => sourceMember.MapFrom((user, response) => response.CreatedAt = user.CreatedOn))
				.ForMember(
					destinationMember => destinationMember.EmailAddress,
					sourceMember =>
						sourceMember.MapFrom((user, response) => response.EmailAddress = user.Email?.ToString()));
				
			CreateMap<CreateUserInput, AppUser>().ForMember(
				destinationMember => destinationMember.Email,
				sourceMember => sourceMember.MapFrom((input, user) => input.EmailAddress = user.Email?.ToString())).ReverseMap();
			
			CreateMap<UpdateUserInput, AppUser>().ForMember(
				destinationMember => destinationMember.Email,
				sourceMember => sourceMember.MapFrom((input, user) => input.EmailAddress = user.Email?.ToString())).ReverseMap();
			
			CreateMap<CreateMessageInput, Message>();
			CreateMap<MessageResponse, Message>().ReverseMap();
			CreateMap<UpdateMessageInput, Message>().ReverseMap();

			CreateMap<BulkUserMessageResponse, Message>().ReverseMap();
			CreateMap<WriteModelResponse, Message>().ReverseMap();
			CreateMap<CreateTimelineInput, Timeline>();
			CreateMap<TimelineResponse, Timeline>().ReverseMap();
			CreateMap<TimelineDetails, TimelineDetailsResponse>().ReverseMap();
			CreateMap<UpdateTimelineInput, Timeline>().ReverseMap();
			CreateMap<CreateRobotArmTimelineClipInput, RobotArmTimelineClip>()
				.ForMember(dest => dest.Type, o => o.MapFrom(src => src.Type))
				.ForMember(dest => dest.StartTime, o => o.MapFrom(src => src.StartTime))
				.ForMember(dest => dest.EndTime, o => o.MapFrom(src => src.EndTime))
				.ForMember(dest => dest.ObjectAction, o => o.MapFrom(src => src.ObjectAction))
				.ForMember(dest => dest.DestinationObjectId, o => o.MapFrom(src => src.DestinationObjectId))
				.ForMember(dest => dest.HandlingObjectId, o => o.MapFrom(src => src.HandlingObjectId));
			
			CreateMap<CreatetimelineTrack, TimelineTrack>()
				.ForMember(dest => dest.sceneObjectId, o => o.MapFrom(src => src.sceneObjectId))
				.ForMember(dest => dest.Clips, o => o.MapFrom(src => src.Clips));


			CreateMap<BulkTimelineDetailsResponse, TimelineDetails>().ReverseMap();
			CreateMap<WriteModelResponse, TimelineDetails>().ReverseMap();
			
		}
	}
}

