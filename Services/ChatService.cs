using ConsolidatedApi.Data;
using ConsolidatedApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsolidatedApi.Services
{
    public class ChatService
    {
        private readonly ConsolidatedDbContext _context;

        public ChatService(ConsolidatedDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(string? organizationId = null)
        {
            var query = _context.ChatMessages.AsQueryable();
            
            if (!string.IsNullOrEmpty(organizationId))
            {
                query = query.Where(m => m.OrganizationId == organizationId);
            }

            return await query.OrderBy(m => m.CreatedAt).ToListAsync();
        }

        public async Task<ChatMessage> CreateMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<UserToUserChat?> GetOrCreateChatAsync(string user1Id, string user2Id)
        {
            var chat = await _context.UserToUserChats
                .FirstOrDefaultAsync(c => 
                    (c.User1Id == user1Id && c.User2Id == user2Id) ||
                    (c.User1Id == user2Id && c.User2Id == user1Id));

            if (chat == null)
            {
                chat = new UserToUserChat
                {
                    User1Id = user1Id,
                    User2Id = user2Id
                };
                _context.UserToUserChats.Add(chat);
                await _context.SaveChangesAsync();
            }

            return chat;
        }

        public async Task<List<UserToUserMessage>> GetUserChatMessagesAsync(string chatId)
        {
            return await _context.UserToUserMessages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserToUserMessage> CreateUserMessageAsync(UserToUserMessage message)
        {
            _context.UserToUserMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}