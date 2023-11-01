using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories;

public class MessageRepository : IMessageRepository
{
	private readonly IMongoCollection<Message> _message;

	public MessageRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
	{
		var database = mongoClient.GetDatabase(settings.DatabaseName);
		_message = database.GetCollection<Message>(settings.MessagesCollectionName);
		
		var indexKeysDefinition = Builders<Message>
			.IndexKeys.Descending(x => x.LastUpdatedAt);
		_message.Indexes.CreateOneAsync(new CreateIndexModel<Message>(indexKeysDefinition));
	}

	public async Task CreateAsync(Message message) =>
		await _message.InsertOneAsync(message);

	public async Task<BulkWriteResult<Message>> CreateManyAsync(List<WriteModel<Message>> messages)
	{
		var writeResult = await _message.BulkWriteAsync(messages);
		return writeResult;
	}
	
	public async Task<IEnumerable<Message>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Message>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			if (isDeleted == false)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _message.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
		else
		{
			if (isDeleted == null || isDeleted == true)
			{
				return await _message.Find(_ => true).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _message.Find(x => x.IsDeleted == false).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
		}
	}

	public async Task<Message?> GetAsync(string id) =>
		await _message.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<Message>> SearchAsync(string search)
	{
		var filter = Builders<Message>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter =  Builders<Message>.Filter.Regex("Title", new BsonRegularExpression(search, "i"));
		}
		return await _message.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
	}
	
	public async Task<IEnumerable<Message>> GetAsyncByOwnerId(string ownerId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Message>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Message>.Filter.Eq(x => x.OwnerId, ownerId);
			if (isDeleted == false)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false); 
			}
			return await _message.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			var filter = Builders<Message>.Filter.Eq(x => x.OwnerId,ownerId);
			if (isDeleted == null || isDeleted == true)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _message.Find(x => x.OwnerId == ownerId).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
	}

	public async Task<IEnumerable<Message>> GetAsyncByTopicId(string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Message>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicId, topicId);
			return await _message.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			var filter = Builders<Message>.Filter.Eq(x => x.TopicId,topicId);
			if (isDeleted == null || isDeleted == true)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _message.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}		
	}

	public async Task<IEnumerable<Message>> GetAsyncByTopicTypeAndTopicId(MessageTopicTypeEnum messageTopicTypeEnum, string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Message>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicType, messageTopicTypeEnum);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicId, topicId);
			if (isDeleted == false)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false); 
			}
			return await _message.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			var filter = Builders<Message>.Filter.Eq(x => x.TopicType, messageTopicTypeEnum);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicId, topicId);
			
			if (isDeleted == null || isDeleted == true)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _message.Find(filter)
            			.SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
	}

	public async Task<IEnumerable<Message>> GetAsyncByMessageTypeAndTopicId(MessageTypeEnum messageTypeEnum, string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Message>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Message>.Filter.Eq(x => x.MessageType, messageTypeEnum);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicId, topicId);
			if (isDeleted == false)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false); 
			}
			return await _message.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			var filter = Builders<Message>.Filter.Eq(x => x.MessageType, messageTypeEnum);
			filter &= Builders<Message>.Filter.Eq(x => x.TopicId, topicId);
			if (isDeleted == null || isDeleted == true)
			{
				filter &= Builders<Message>.Filter.Eq(x => x.IsDeleted , false);
			}
			
			return await _message.Find(x => x.MessageType == messageTypeEnum && x.TopicId == topicId)
            			.SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
	}


	public async Task RemoveAsync(string id, string? usernameActor) =>
		await _message.UpdateOneAsync(msg => msg.Id == id,
			Builders<Message>.Update
				.Set(msg => msg.IsDeleted, true)
				.Set(msg => msg.LastUpdatedAt,DateTimeOffset.UtcNow));

	public async Task UpdateAsync(string id, Message updatedMessage) =>
		await _message.ReplaceOneAsync(x => x.Id == id, updatedMessage);
}

