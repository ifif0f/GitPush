using System.Text.Json.Serialization;

namespace WebGitPush.Models.Parking
{
    public class ParkingComplex
    {
        public int complex_id { get; set; }

        public string complex_title { get; set; }

        public List<ParkingBuilding> buildings { get; set; }
    }
}
