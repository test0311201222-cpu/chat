using System.Collections.Generic;

namespace chat.Models
{
    public class AssistantProfile
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Tone { get; set; }
        public string Goal { get; set; }
        public List<string> Rules { get; set; } = new List<string>();
    }
}
