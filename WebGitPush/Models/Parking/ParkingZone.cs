using System.Text.Json.Serialization;

namespace WebGitPush.Models.Parking
{
    public class ParkingZone
    {
        public string id { get; set; }

        public string name { get; set; }

        public List<ParkingSpot> spots { get; set; }
    }
}
