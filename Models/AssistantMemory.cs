using System.Collections.Generic;

namespace chat.Models
{
    public class AssistantMemory
    {
        public string UserName { get; set; } = "amigo";
        public string LastMode { get; set; } = "Geral";
        public List<string> FavoriteTopics { get; set; } = new List<string>();
    }
}
