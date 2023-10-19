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
	}

	public async Task CreateAsync(Message message) =>
		await _message.InsertOneAsync(message);

	public async Task<IEnumerable<Message>> GetAsync() =>
		await _message.Find(_ => true).ToListAsync();

	public async Task<Message?> GetAsync(string id) =>
		await _message.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<Message>> SearchAsync(string search)
	{
		var filter = Builders<Message>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter =  Builders<Message>.Filter.Regex("Title", new BsonRegularExpression(search, "i"));
		}
		return await _message.Find(filter).ToListAsync();
	}
	
	public async Task<IEnumerable<Message>> GetAsyncByOwnerId(string ownerId) =>
		await _message.Find(x => x.OwnerId == ownerId).ToListAsync();
	
	public async Task<IEnumerable<Message>> GetAsyncByTopicId(string topicId) =>
		await _message.Find(x => x.TopicId == topicId).ToListAsync();

	public async Task<IEnumerable<Message>> GetAsyncByTopicTypeAndTopicId(MessageTopicTypeEnum messageTopicTypeEnum, string topicId) =>
		await _message.Find(x => x.TopicType == messageTopicTypeEnum && x.TopicId == topicId).ToListAsync();
	
	public async Task<IEnumerable<Message>> GetAsyncByMessageTypeAndTopicId(MessageTypeEnum messageTypeEnum, string topicId) =>
		await _message.Find(x => x.MessageType == messageTypeEnum && x.TopicId == topicId).ToListAsync();


	public async Task RemoveAsync(string id) =>
		await _message.DeleteOneAsync(x => x.Id == id);

	public async Task UpdateAsync(string id, Message updatedMessage) =>
		await _message.ReplaceOneAsync(x => x.Id == id, updatedMessage);
}

