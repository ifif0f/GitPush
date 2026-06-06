using System.Text.Json.Serialization;

namespace WebGitPush.Models.Storages
{
    public class StorageBuilding
    {
        public int building_id { get; set; }

        public string building_title { get; set; }

        public List<StorageRoom> storages { get; set; }
    }
}
