using WebGitPush.Classes;
using WebGitPush.Models.News;
using WebGitPush.Models.Parking;
using WebGitPush.Models.Storages;

namespace WebGitPush.ViewModels
{
    public class VMApp
    {
        public ParkingApiResponse? ParkingData { get; set; }
        public StorageApiResponse? StorageData { get; set; }

        public List<BuildingRef> ParkingBuildings { get; set; } = new();
        public List<BuildingRef> StorageBuildings { get; set; } = new();

        public int SelectedParkingBuildingId { get; set; }
        public int SelectedStorageBuildingId { get; set; }
    }
}
