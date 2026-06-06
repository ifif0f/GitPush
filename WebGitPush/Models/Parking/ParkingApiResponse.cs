using System.Text.Json.Serialization;

namespace WebGitPush.Models.Parking
{
    public class ParkingApiResponse
    {
        public string command { get; set; }

        public string message { get; set; }

        public int error { get; set; }

        public ParkingData data { get; set; }
    }
}
