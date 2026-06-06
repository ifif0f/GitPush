using WebGitPush.Classes;
using WebGitPush.Models.News;

namespace WebGitPush.ViewModels
{
    public class VMIndex
    {
        private readonly ConnectionApi connectionApi;
        public VMApp ViewModel { get; set; } = new();
        public List<NewsItem> newsItems { get; set; } = new();


        public async Task OnGetAsync(int parkingBuildingId = 0, int storageBuildingId = 0)
        {
            var parkingData = await connectionApi.GetParkingDataAsync();
            var storageData = await connectionApi.GetStorageDataAsync();
            var newsData = await connectionApi.GetNewsDataAsync();

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

            newsItems = ProcessNews(newsData);

            //UpdateParkingDisplay();
            //UpdateStorageDisplay();
        }

        private List<NewsItem> ProcessNews(NewsApiResponse? newsData, int maxCount = 6)
        {
            var newsList = new List<NewsItem>();

            if (newsData?.error != 0 || newsData.data?.items == null)
                return newsList;

            foreach (var news in newsData.data.items)
            {
                if (news.is_external) continue;

                newsList.Add(new NewsItem
                {
                    id = news.id,
                    title = news.title,
                    text = news.text,
                    created_at = DateTime.Now,
                    published_at = DateTime.Now,
                    buildings = news.buildings.Select(b => b.id).ToList(),
                    images = ""
                });
            }

            return newsList.OrderByDescending(n => n.DisplayDate).Take(maxCount).ToList();
        }
    }
}
