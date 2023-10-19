using Domain.Entities;
using Domain.Helper;

namespace domain.Repositories
{
	public interface IMessageRepository
	{
		Task<IEnumerable<Message>> GetAsync();
		Task<Message> GetAsync(string id);
		Task<IEnumerable<Message>> SearchAsync(string search);
		Task CreateAsync(Message message);
		Task UpdateAsync(string id, Message updatedMessage);
		Task RemoveAsync(string id);
		Task<IEnumerable<Message>> GetAsyncByOwnerId(string ownerId);
		
		Task<IEnumerable<Message>> GetAsyncByTopicId(string topicId);

        Task<IEnumerable<Message>> GetAsyncByTopicTypeAndTopicId(MessageTopicTypeEnum topicTypeEnum, string topicId);
        
        Task<IEnumerable<Message>> GetAsyncByMessageTypeAndTopicId(MessageTypeEnum messageTypeEnum, string topicId);
    }
}

