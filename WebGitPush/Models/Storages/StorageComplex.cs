using System.Text.Json.Serialization;

namespace WebGitPush.Models.Storages
{
    public class StorageComplex
    {
        public int complex_id { get; set; }

        public string complex_title { get; set; }

        public List<StorageBuilding> buildings { get; set; }
    }
}
