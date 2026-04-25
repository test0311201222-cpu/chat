namespace chat.Models
{
    public class AppSettings
    {
        public string OpenAiApiKey { get; set; } = string.Empty;
        public string OpenAiModel { get; set; } = "gpt-5-mini";
    }
}
