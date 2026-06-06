using System.Text.Json.Serialization;

namespace WebGitPush.Models.Storages
{
    public class StorageApiResponse
    {
        public string command { get; set; }

        public string message { get; set; }

        public int error { get; set; }

        public StorageData data { get; set; }
    }
}
