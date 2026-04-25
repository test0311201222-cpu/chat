using System.Collections.Generic;

namespace chat.Models
{
    public class KnowledgeItem
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public List<string> Keywords { get; set; } = new List<string>();
        public string Content { get; set; }
    }
}
