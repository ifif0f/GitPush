using System.Text.Json.Serialization;

namespace WebGitPush.Models.News
{
    public class NewsItem
    {
        public int id { get; set; }

        //public int status { get; set; }

        public string title { get; set; } = string.Empty;

        public string text { get; set; } = string.Empty;

        //public string date { get; set; } = string.Empty;

        public string created_at { get; set; } = string.Empty;

        public string published_at { get; set; } = string.Empty;

        //public string? expires_at { get; set; }

        //public string? scheduled_for { get; set; }

        public List<string> images { get; set; } = new();

        //public NewsType type { get; set; } = new();

        public List<BuildingRef> buildings { get; set; } = new();

        //public string? remote_id { get; set; }

        //public bool is_external { get; set; }

        //public object? targeting { get; set; }
    }
}
