using System;
using System.Numerics;
using System.Text.Json.Serialization;
using domain.Dto;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Dto
{
    public class CreateBulkUserMessageInput
    {
        public string[] OwnerId { get; set; }
		
        public string Title { get; set; }
        
        public string Body { get; set; }
    }
}