using System;

namespace chat.Models
{
    public class ConversationTurn
    {
        public string Role { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
