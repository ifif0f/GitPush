using System.Text.Json.Serialization;

namespace WebGitPush.Models.News
{
    public class NewsApiResponse
    {
        public string command { get; set; } = string.Empty;

        public string message { get; set; } = string.Empty;

        public int error { get; set; }

        public NewsData data { get; set; } = new();
    }
}
