using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

          await  _context.Messages.AddAsync(new Message
            {
                UserId = GetUserId(),
                Text = message,
                Data = DateTime.Now

            });

          await  _context.SaveChangesAsync();

            await Clients.All.SendAsync("Receive", Context.User.Identity.Name, DateTime.Now.ToString("dd.MM HH:mm"),  message);
        }

        [Authorize]
        public async Task DeleteMessage(int id)
        {
            var message = _context.Messages.FirstOrDefault(x => x.Id == id);

            if (message != null)
          {
                if (message.UserId == GetUserId() || Context.User.IsInRole(UserRole.Admin.ToString()))
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
         }

            await Clients.All.SendAsync("HideMessage", message.Id);
        }

        private int GetUserId()
        {
            var userIdstr = Context.User.FindFirst("Id")?.Value;
            var userId = Convert.ToInt32(userIdstr);

            return userId;
        }
    }

}
