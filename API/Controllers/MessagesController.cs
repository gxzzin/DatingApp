using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;

        public MessagesController(IMapper mapper, IUnitOfWork uof)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var userName = User.GetUserName();
            if (userName == createMessageDto.RecipientUserName.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _uof.UserRepository.GetUserByUserNameAsync(userName);
            var recipient = await _uof.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content

            };

            _uof.MessageRepository.AddMessage(message);

            if (await _uof.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();

            var messages = await _uof.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var userName = User.GetUserName();
            var message = await _uof.MessageRepository.GetMessage(id);

            if (message.Sender.UserName != userName && message.Recipient.UserName != userName)
                return Unauthorized();

            if(message.Sender.UserName == userName) message.SenderDeleted = true;
            
            if(message.Recipient.UserName == userName) message.RecipientDeleted = true;
            
            if(message.SenderDeleted && message.RecipientDeleted) _uof.MessageRepository.DeleteMessage(message);

            if(await _uof.Complete()) return Ok();

            return BadRequest("Problem deleting the message!");
        }

    }
}