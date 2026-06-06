using System.Text.Json.Serialization;

namespace WebGitPush.Models.Parking
{
    public class ParkingBuilding
    {
        public int building_id { get; set; }

        public string building_title { get; set; }

        public List<ParkingZone> zones { get; set; }
    }
}
