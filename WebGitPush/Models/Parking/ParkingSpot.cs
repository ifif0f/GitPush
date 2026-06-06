using System.Text.Json.Serialization;

namespace WebGitPush.Models.Parking
{
    public class ParkingSpot
    {
        public string id { get; set; }

        public string assignment_type { get; set; }

        public string status { get; set; }
    }
}
