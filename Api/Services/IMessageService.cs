using System;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;

namespace khi_robocross_api.Services
{
	public interface IMessageService
	{
		 ValueTask<IEnumerable<MessageResponse>> GetAllMessages();
		 ValueTask<MessageResponse> GetMessageById(String id);
		 Task AddMessage(Message client);
         Task UpdateMessage(string id, UpdateMessageInput updatedMessage);
         Task RemoveMessage(string id);
         ValueTask<IEnumerable<MessageResponse>> Query(string search);
         ValueTask<IEnumerable<MessageResponse>> GetMessageByOwnerId(string ownerId);
         
         ValueTask<IEnumerable<MessageResponse>> GetMessageByTopicId(string topicId);
         ValueTask<IEnumerable<MessageResponse>> GetMessageByTopicTypeAndTopicId(MessageTopicTypeEnum topicType, string topicId);
         
         ValueTask<IEnumerable<MessageResponse>> GetMessageByMessageTypeAndTopicId(MessageTypeEnum messageType, string topicId);
	}
}

