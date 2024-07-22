using AutoMapper;
using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Models;
using cattoapi.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using MimeKit;
using System.Security.Claims;

namespace SignalRChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

        private static readonly Dictionary<int, string> _userIdConnectionIdMap = new Dictionary<int, string>();
        private readonly CattoDbContext _context;
        private readonly TokensRepo _tokensRepo;
        private readonly IMapper _mapper;

        public ChatHub(CattoDbContext context, TokensRepo tokensRepo, IMapper mapper)
        {
            _context = context;
            _tokensRepo = tokensRepo;
            _mapper = mapper;
        }

        private async Task<int> VerifyToken()
        {
          return await _tokensRepo.IsTokenValid(Context.GetHttpContext().Request.Query["access_token"]);
        }

        public async Task<CustomResponse<bool>> SendMessage(int receiverId, string messageText)
        {
            if (messageText.Trim().Length == 0)
                return new CustomResponse<bool>(400,"message can't be empty");

            int senderId = await VerifyToken();
            if (senderId == 0)
            {
                Context.Abort();
                return new CustomResponse<bool>(401, "Session expired");
            }

            Message message = new Message();
            message.SenderId = senderId;
            Conversation conversation;
            if (senderId < receiverId)
            {
                conversation = _context.Conversations.FirstOrDefault(c => c.Participant1Id == senderId && c.Participant2Id == receiverId);
                message.Participant1Id = senderId;
                message.Participant2Id = receiverId;
            }
            else
            {
                conversation = _context.Conversations.FirstOrDefault(c => c.Participant1Id == receiverId && c.Participant2Id == senderId);
                message.Participant1Id = receiverId;
                message.Participant2Id = senderId;
            }

            if (conversation == null)
                 if((await CreateConvo(receiverId)).responseCode != 201)
                     return new CustomResponse<bool>(500, "internal error");







            message.Content = messageText;

            try
            {
                if (!_userIdConnectionIdMap.TryGetValue(receiverId, out string receiverConnectionId))
                {
                    message.Pending = true;
                    _context.Messages.Add(message);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Messages.Add(message);
                    await _context.SaveChangesAsync();
                    MessageDTO messageDTO = _mapper.Map<MessageDTO>(message);
                    Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, messageDTO);
                }

                return new CustomResponse<bool>(201,"Message sent successfully");

            }
            catch
            {
                return new CustomResponse<bool>(500, "internal error");
            }

        }
        
        public async Task<CustomResponse<IEnumerable<ConversationDTO>>> ViewConverSations(int take, int skip)
        {
            int senderId = await VerifyToken();
            if (senderId == 0)
            {
                Context.Abort();
                return new CustomResponse<IEnumerable<ConversationDTO>>(401, "Session expired");
            }

            IEnumerable<Conversation> conversations = _context.Conversations.Where(c => c.Participant1Id == senderId || c.Participant2Id == senderId).Include(c => c.Messages).Select(c => new
            {
                Conversation = c,
                LastMessageTime = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault(c => c.Participant1Id == c.Participant1Id || c.Participant2Id == senderId).Timestamp
            }).OrderByDescending(c => c.LastMessageTime).Select(c=>c.Conversation).Skip(skip).Take(take);




            if (conversations.Count() == 0)
                return  new CustomResponse<IEnumerable<ConversationDTO>>(404, "Not found");



            _userIdConnectionIdMap.TryGetValue(senderId, out string receiverConnectionId);



          IEnumerable<ConversationDTO> conversationsDTO = _mapper.Map<IEnumerable<ConversationDTO>>(conversations);



            return new CustomResponse<IEnumerable<ConversationDTO>>(200, "Conversations retrieved successfully", conversationsDTO);

        }

        public async Task<CustomResponse<bool>> CreateConvo(int receiverId)
        {
            int senderId = await VerifyToken();
            if ( senderId == 0)
            {
                Context.Abort();
                return new CustomResponse<bool>(401,"session expired");
            }
                

            Conversation conversation = new Conversation();

            if(senderId < receiverId)
            {
                conversation.Participant1Id = senderId;
                conversation.Participant2Id = receiverId;
            }
            else
            {
                conversation.Participant1Id = receiverId;
                conversation.Participant2Id = senderId;
            }

         
            try
            {
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(201, "Conversation Created succesffully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal error");
            }
        }

        private async Task<bool> SendPendingMessages(int userId,int take, int skip)
        {
            var messages = _context.Messages.Where(M => (M.Participant1Id == userId || M.Participant2Id == userId) && M.Pending == true).OrderBy(M => M.Timestamp).Skip(skip).Take(take).ToList();

            if (messages.Count() == 0)
                return false;

            messages.ForEach(M => M.Pending = false);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }


            _userIdConnectionIdMap.TryGetValue(userId, out string receiverConnectionId);

            IEnumerable<MessageDTO> messagesDTO = _mapper.Map<IEnumerable<MessageDTO>>(messages);

            Clients.Client(receiverConnectionId).SendAsync("ReceivePendingMessages", new CustomResponse<IEnumerable<MessageDTO>>(200, "Pedning messages", messagesDTO));


            if (messages.Count() < take)
                return false;



            return true;
        }

        public override async Task OnConnectedAsync()
        {
            int userId;
            try
            {
                userId = int.Parse(Context.UserIdentifier);

                _userIdConnectionIdMap[userId] = Context.ConnectionId;
            }
            catch
            {
                Context.Abort();
                return;
            }

            await base.OnConnectedAsync();

            int take = 15;
            int skip = 0;
            while(await SendPendingMessages(userId, take, skip))
            {
                skip += 15;
            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            int accountId = await VerifyToken();
            _userIdConnectionIdMap.Remove(accountId);
            

            await base.OnDisconnectedAsync(exception);
        }
    }
}