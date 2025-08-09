using Microsoft.JSInterop;
using Newtonsoft.Json;
using UltimateWeapons.Cycleables.Weapons;
using static Website.Pages.Stones;

namespace Website.Services
{
    public class PersistenceService(IJSRuntime jsRuntime)
    {

        public async Task<PageData> TryLoadDataFromLocalStorageAsync()
        {
            try
            {
                // Try to load from localStorage
                var storedData = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "stones_data");

                if (!string.IsNullOrEmpty(storedData))
                {
                    var res = JsonConvert.DeserializeObject<PageData>(storedData, TypeNameSerializerSettings()) ?? new PageData();
                    var bh = res.Uws.FirstOrDefault(uw => uw.GetType() == typeof(BlackHole));
                    // fix up link between UW and tower stats
                    if (bh is BlackHole bhUw)
                    {
                        bhUw.TowerStats = res.TowerStats;
                    }
                    return res;
                }
                else
                {
                    return new PageData();
                }
            }
            catch (Exception)
            {
                // If loading fails, initialize with default data
                return new PageData();
            }
        }

        public async Task SaveDataToLocalStorageAsync(PageData pageData)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(pageData, TypeNameSerializerSettings());
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "stones_data", jsonString);
            }
            catch (Exception ex)
            {
                // Handle localStorage save error if needed
                Console.WriteLine($"Failed to save to localStorage: {ex.Message}");
            }
        }

        private static JsonSerializerSettings TypeNameSerializerSettings()
        {
            return new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
        }
    }
}
