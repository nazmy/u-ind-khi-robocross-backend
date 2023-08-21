﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Entities
{
	public class Client
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null;
    }
}

