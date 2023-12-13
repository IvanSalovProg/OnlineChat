using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OnlineChatMvc.Data;

namespace OnlineChatMvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _context;

        public ChatHub(ChatContext context) 
        {
            _context = context;
        }

        [Authorize]
        public async Task Send(string message)
        {
            var userIdstr = Context.User.FindFirst("Id")?.Value;
            var userId = Convert.ToInt32(userIdstr);

          await  _context.Messages.AddAsync(new Message
            {
                UserId = userId,
                Text = message,
                Data = DateTime.Now

            });

          await  _context.SaveChangesAsync();

            await Clients.All.SendAsync("Receive", Context.User.Identity.Name, DateTime.Now,  message);
        }

    }

}
