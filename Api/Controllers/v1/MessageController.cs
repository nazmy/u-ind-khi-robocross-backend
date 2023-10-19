using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMessageService messageService,
        IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
        _logger = new LoggerFactory().CreateLogger<MessageController>();
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        IEnumerable<MessageResponse> messageList = await _messageService.GetAllMessages();
        return Ok(messageList);
    }
    
    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<MessageResponse>> Get(string id)
    {
        try
        {
            MessageResponse message = await _messageService.GetMessageById(id);
            if (message == null)
            {
                return NotFound($"Message with Id = {id} not found");
            }

            return Ok(message);
        }
        catch (ArgumentException aex)
        {
            return BadRequest("Invalid Message ID");
        }
    }
    
    [HttpGet("Search")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageResponse>))]
    public async Task<ActionResult<MessageResponse>> Search([FromQuery(Name = "Title")] string search)
    {
        try
        {
            var messageList = await _messageService.Query(search);
            return Ok(messageList);
        }
        catch (Exception e)
        { 
            _logger.LogError($"Error on V1 GetClient API :{e.StackTrace.ToString()}");
            throw;
        }
    }
    
    [HttpGet("Owner/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageResponse>))]
    public async Task<ActionResult<MessageResponse>> GetMessageByOwnerId(string ownerId)
    {
        try
        {
            var messageList = await _messageService.GetMessageByOwnerId(ownerId);
            return Ok(messageList);
        }
        catch (Exception e)
        { 
            _logger.LogError($"Error on V1 GetClient API :{e.StackTrace.ToString()}");
            throw;
        }
    }
    
    [HttpGet("Topic/{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageResponse>))]
    public async Task<ActionResult<MessageResponse>> GetMessageByTopicId(string topicId)
    {
        try
        {
            var messageList = await _messageService.GetMessageByTopicId(topicId);
            return Ok(messageList);
        }
        catch (Exception e)
        { 
            _logger.LogError($"Error on V1 Get Message By Topic Id API :{e.StackTrace.ToString()}");
            throw;
        }
    }
    
    [HttpGet("TopicType/{topicType}/Topic/{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageResponse>))]
    public async Task<ActionResult<MessageResponse>> GetMessageByTopicTypeAndTopicId(MessageTopicTypeEnum topicType, string topicId)
    {
        try
        {
            var messageList = await _messageService.GetMessageByTopicTypeAndTopicId(topicType, topicId);
            return Ok(messageList);
        }
        catch (Exception e)
        { 
            _logger.LogError($"Error on V1 Get Message By TopicType and Topic Id API :{e.StackTrace.ToString()}");
            throw;
        }
    }
    
    [HttpGet("MessageType/{messageType}/Topic/{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageResponse>))]
    public async Task<ActionResult<MessageResponse>> GetMessageByMessageTypeAndTopicId(MessageTypeEnum messageType, string topicId)
    {
        try
        {
            var messageList = await _messageService.GetMessageByMessageTypeAndTopicId(messageType, topicId);
            return Ok(messageList);
        }
        catch (Exception e)
        { 
            _logger.LogError($"Error on V1 Get Message By Message Type and Topic Id API :{e.StackTrace.ToString()}");
            throw;
        }
    }
    
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] CreateMessageInput newMessage)
    {
        if (newMessage == null)
        {
            _logger.LogWarning("V1 Create Message API BadRequest Message Data");
            return BadRequest(ModelState);
        }
            
        var message = _mapper.Map<Message>(newMessage);
        await _messageService.AddMessage(message);

        return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
    }
    
    [HttpPut("{id:length(24)}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, UpdateMessageInput updatedMessage)
    {
        try
        {
            if (updatedMessage == null)
                return BadRequest(ModelState);

            await _messageService.UpdateMessage(id, updatedMessage);

            return NoContent();
        }
        catch(ArgumentException aex)
        {
            return BadRequest(aex.Message);
        }
        catch (KeyNotFoundException kex)
        {
            return NotFound(kex.Message);
        }
    }
    
    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        var client = await _messageService.GetMessageById(id);

        if (client is null)
        {
            return NotFound($"Message with Id = {id} not found");
        }

        await _messageService.RemoveMessage(id);
        return Ok($"Message with Id = {id} deleted");
    }
}