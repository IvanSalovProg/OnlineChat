﻿namespace OnlineChatMvc.Data
{
    public class Message
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public DateTime Data {  get; set; }
    }
}
