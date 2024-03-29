﻿using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;
using domain.Repositories;
using khi_robocross_api.Helper;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace khi_robocross_api.Services
{
	public class MessageService : IMessageService
	{
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public MessageService(IMessageRepository messageRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _messageRepository = messageRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddMessage(Message inputMessage)
        {
            if (inputMessage == null)
                throw new ArgumentException("Message input is invalid");

            inputMessage.CreateChangesTime(inputMessage,  IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            
            //validation goes here
            await _messageRepository.CreateAsync(inputMessage);
        }
        
        public async ValueTask<List<BulkUserMessageResponse>> AddBulkMessage(CreateBulkUserMessageInput createBulkUserMessageInput)
        {
            if (createBulkUserMessageInput == null)
                throw new ArgumentException("Message input is invalid");

            List<WriteModel<Message>> messageList = new List<WriteModel<Message>>();
            foreach (string userId in createBulkUserMessageInput.OwnerIds)
            {
                Message newMessage = new Message();
                newMessage.MessageType = MessageTypeEnum.Notification;
                newMessage.TopicType = MessageTopicTypeEnum.User;
                newMessage.TopicId = userId;
                newMessage.OwnerId = userId;
                newMessage.Title = createBulkUserMessageInput.Title;
                newMessage.Body = createBulkUserMessageInput.Body;
                newMessage.CreateChangesTime(newMessage, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
                messageList.Add(new InsertOneModel<Message>(newMessage));
            }
            
            //validation goes here
             var resultWrites =  await _messageRepository.CreateManyAsync(messageList);
             var insertedUserNotification = resultWrites.ProcessedRequests.ToList();
             List<BulkUserMessageResponse> userMessageResponses = new List<BulkUserMessageResponse>();
             
             foreach (var createdMessage in insertedUserNotification)
             { 
                 string createdMessageString = JsonConvert.SerializeObject(createdMessage);
                 var message = JsonConvert.DeserializeObject<WriteModelWrapper<BulkUserMessageResponse>>(createdMessageString);
                 userMessageResponses.Add(message.Document);
             }
             return userMessageResponses;
        }

        public async ValueTask<IEnumerable<MessageResponse>> GetAllMessages(DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            var messageTask = await _messageRepository.GetAsync(lastUpdatedAt, isDeleted);
            if (messageTask != null)
                return _mapper.Map<IEnumerable<MessageResponse>>(messageTask.ToList());

            return null;
        }

        public async ValueTask<MessageResponse> GetMessageById(string id)
        {
            if (id == null)
                throw new ArgumentException("Message Id is Invalid");

            var messageTask = await _messageRepository.GetAsync(id);
            return _mapper.Map<MessageResponse>(messageTask);
        }
        
        public async ValueTask<IEnumerable<MessageResponse>> Query(string search)
        {
            var messageTask = await _messageRepository.SearchAsync(search);
            return _mapper.Map<IEnumerable<MessageResponse>>(messageTask.ToList());
        }
        
        public async ValueTask<IEnumerable<MessageResponse>> GetMessageByOwnerId(string ownerId, DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            if (ownerId == null)
                throw new ArgumentException("Owner Id is Invalid");

            var messageTask = await _messageRepository.GetAsyncByOwnerId(ownerId, lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<MessageResponse>>(messageTask);
        }
        
        public async ValueTask<IEnumerable<MessageResponse>> GetMessageByTopicTypeAndTopicId(MessageTopicTypeEnum topicType, string topicId, DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            if (topicId == null)
                throw new ArgumentException("Topic Id is Invalid");

            var messageTask = await _messageRepository.GetAsyncByTopicTypeAndTopicId(topicType, topicId,lastUpdatedAt,isDeleted);
            return _mapper.Map<IEnumerable<MessageResponse>>(messageTask);
        }
        
        public async ValueTask<IEnumerable<MessageResponse>> GetMessageByTopicId(string topicId,DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            if (topicId == null)
                throw new ArgumentException("Topic Id is Invalid");

            var messageTask = await _messageRepository.GetAsyncByTopicId(topicId,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<MessageResponse>>(messageTask);
        }
        
        public async ValueTask<IEnumerable<MessageResponse>> GetMessageByMessageTypeAndTopicId(MessageTypeEnum messageType, string topicId,DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            if (topicId == null)
                throw new ArgumentException("Topic Id is Invalid");

            var messageTask = await _messageRepository.GetAsyncByMessageTypeAndTopicId(messageType, topicId,lastUpdatedAt,isDeleted);
            return _mapper.Map<IEnumerable<MessageResponse>>(messageTask);
        }
        
        public async Task RemoveMessage(string id)
        {
            if (id == null)
                throw new ArgumentException("Message Id is Invalid");

            await _messageRepository.RemoveAsync(id,IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
        }

        public async Task UpdateMessage(string id, UpdateMessageInput updateMessageInput)
        {
            if (id == null)
                throw new ArgumentException("Message Id is invalid");

            if (updateMessageInput == null)
                throw new ArgumentException("Message Input is invalid");
            
            var message = await _messageRepository.GetAsync(id);

            if (message == null)
                throw new KeyNotFoundException($"Message with Id = {id} not found");

            _mapper.Map<UpdateMessageInput,Message>(updateMessageInput,message);
            message.UpdateChangesTime(message, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            await _messageRepository.UpdateAsync(id, message);
        }
    }
}

