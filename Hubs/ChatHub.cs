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

        public async Task<int> VerifyToken()
        {
          return await _tokensRepo.IsTokenValid(Context.GetHttpContext().Request.Query["access_token"]);
        }
        public async Task<bool> SendMessage(int receiverId, string messageText)
        {
            if (messageText.Trim().Length == 0)
                return false;

            int senderId = await VerifyToken();
            if (senderId == 0)
            {
                Context.Abort();
                return false;
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
                 if(!await CreateConvo(receiverId))
                    return false;
               



            


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

                return true;

            }
            catch
            {
                return false;
            }

        }
        
        public async Task<bool> ViewConverSations(int take, int skip)
        {
            int senderId = await VerifyToken();
            if (senderId == 0)
            {
                Context.Abort();
                return false;
            }

            IEnumerable<Conversation> conversations = _context.Conversations.Where(c => c.Participant1Id == senderId || c.Participant2Id == senderId).Include(c => c.Messages).Select(c => new
            {
                Conversation = c,
                LastMessageTime = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault(c => c.Participant1Id == c.Participant1Id || c.Participant2Id == senderId).Timestamp
            }).OrderByDescending(c => c.LastMessageTime).Select(c=>c.Conversation).Skip(skip).Take(take);




            if (conversations.Count() == 0)
                return false;



            _userIdConnectionIdMap.TryGetValue(senderId, out string receiverConnectionId);



            Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", _mapper.Map<IEnumerable<ConversationDTO>>(conversations));
            


            return true;

        }

        public async Task<bool> CreateConvo(int receiverId)
        {
            int senderId = await VerifyToken();
            if ( senderId == 0)
            {
                Context.Abort();
                return false;
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> SendPendingMessages(int userId,int take, int skip)
        {
            var messages = _context.Messages.Where(M => (M.Participant1Id == userId || M.Participant2Id == userId) && M.Pending == true).Skip(skip).Take(take).ToList();

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

            Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", _mapper.Map<IEnumerable<MessageDTO>>(messages),"HEHE BOI");

            
            if(messages.Count() < take)
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