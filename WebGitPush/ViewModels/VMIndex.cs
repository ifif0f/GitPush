using WebGitPush.Classes;
using WebGitPush.Models.News;
using WebGitPush.Models.Parking;
using WebGitPush.Models.Storages;

namespace WebGitPush.ViewModels
{
    public class VMIndex
    {
        private readonly ConnectionApi _connectionApi;

        public VMIndex()
        {
            _connectionApi = new ConnectionApi();
        }

        public VMApp ViewModel { get; set; } = new();
        public List<NewsItem> NewsItems { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task LoadDataAsync(int parkingBuildingId = 0, int storageBuildingId = 0)
        {
            try
            {
                var parkingData = await _connectionApi.GetParkingDataAsync();
                var storageData = await _connectionApi.GetStorageDataAsync();
                var newsData = await _connectionApi.GetNewsDataAsync();

                ViewModel.ParkingData = parkingData;
                ViewModel.StorageData = storageData;

                if (parkingData?.data?.items != null)
                {
                    foreach (var complex in parkingData.data.items)
                    {
                        foreach (var building in complex.buildings)
                        {
                            ViewModel.ParkingBuildings.Add(new BuildingRef
                            {
                                id = building.building_id,
                                title = building.building_title
                            });
                        }
                    }
                }

                if (storageData?.data?.items != null)
                {
                    foreach (var complex in storageData.data.items)
                    {
                        foreach (var building in complex.buildings)
                        {
                            ViewModel.StorageBuildings.Add(new BuildingRef
                            {
                                id = building.building_id,
                                title = building.building_title
                            });
                        }
                    }
                }

                ViewModel.SelectedParkingBuildingId = parkingBuildingId > 0 ? parkingBuildingId :
                    ViewModel.ParkingBuildings.FirstOrDefault()?.id ?? 0;

                ViewModel.SelectedStorageBuildingId = storageBuildingId > 0 ? storageBuildingId :
                    ViewModel.StorageBuildings.FirstOrDefault()?.id ?? 0;

                NewsItems = ProcessNews(newsData);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки данных: {ex.Message}";
            }
        }

        private List<NewsItem> ProcessNews(NewsApiResponse? newsData, int maxCount = 6)
        {
            var newsList = new List<NewsItem>();

            if (newsData?.error != 0 || newsData.data?.items == null)
                return newsList;

            foreach (var news in newsData.data.items)
            {
                if (news.is_external) continue;

                newsList.Add(news);
            }

            return newsList
                .OrderByDescending(n => n.published_at)
                .Take(maxCount)
                .ToList();
        }

        public int GetFreeParkingSpotsCount(int buildingId)
        {
            if (ViewModel.ParkingData?.data?.items == null) return 0;

            int freeCount = 0;
            foreach (var complex in ViewModel.ParkingData.data.items)
            {
                foreach (var building in complex.buildings)
                {
                    if (building.building_id == buildingId)
                    {
                        foreach (var zone in building.zones ?? new List<ParkingZone>())
                        {
                            foreach (var spot in zone.spots ?? new List<ParkingSpot>())
                            {
                                if (spot.status == "free") freeCount++;
                            }
                        }
                    }
                }
            }
            return freeCount;
        }

        public (int publicStatus, int unassigned, int privateStatus) GetStorageFullStats(int buildingId)
        {
            int publicCount = 0;
            int unassignedCount = 0;
            int privateCount = 0;

            if (ViewModel.StorageData?.data?.items == null) return (0, 0, 0);

            foreach (var complex in ViewModel.StorageData.data.items)
            {
                foreach (var building in complex.buildings)
                {
                    if (building.building_id == buildingId)
                    {
                        foreach (var room in building.storages ?? new List<StorageRoom>())
                        {
                            switch (room.assignment_type)
                            {
                                case "public":
                                    publicCount++;
                                    break;
                                case "unassigned":
                                    unassignedCount++;
                                    break;
                                case "private":
                                    privateCount++;
                                    break;
                            }
                        }
                    }
                }
            }
            return (publicCount, unassignedCount, privateCount);
        }
    }
}