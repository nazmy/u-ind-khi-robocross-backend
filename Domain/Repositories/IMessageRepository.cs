using Domain.Entities;
using Domain.Helper;
using MongoDB.Driver;

namespace domain.Repositories
{
	public interface IMessageRepository
	{
		Task<IEnumerable<Message>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Message> GetAsync(string id);
		Task<IEnumerable<Message>> SearchAsync(string search);
		Task CreateAsync(Message message);
		Task<BulkWriteResult<Message>> CreateManyAsync(List<WriteModel<Message>> messages);
		Task UpdateAsync(string id, Message updatedMessage);
		Task RemoveAsync(string id, string? usernameActor);
		Task<IEnumerable<Message>> GetAsyncByOwnerId(string ownerId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		
		Task<IEnumerable<Message>> GetAsyncByTopicId(string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);

        Task<IEnumerable<Message>> GetAsyncByTopicTypeAndTopicId(MessageTopicTypeEnum topicTypeEnum, string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
        
        Task<IEnumerable<Message>> GetAsyncByMessageTypeAndTopicId(MessageTypeEnum messageTypeEnum, string topicId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}

