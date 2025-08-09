using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UltimateWeapons;
using UltimateWeapons.Cycleables;
using UltimateWeapons.Cycleables.Weapons;
using Website.Services;

namespace Website.Pages
{
    public partial class Stones(PersistenceService persistenceService, InternalDataLookup internalDataLookup)
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        public class PageData
        {
            public List<UltimateWeapon> Uws { get; set; } = new();
            public TowerStats TowerStats { get; set; } = new TowerStats(69.5m);

        }

        private PageData data = new();

        private string selectBox_UWName = "";


        private static string[] AllUWs = [
            "Golden Tower",
            "Black Hole",
            "Death Wave",
            //"Golden Bot",
            //"Spotlight"
            ];
        private List<string> AvailableUWs = [];
        private bool DisableAddingUWs => AvailableUWs.Count < 1;
        public UltimateWeaponCycling.CycleInfo CycleInfo { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            AvailableUWs = new(AllUWs);
            await InitPageAsync();
            SelectNextUW();
        }

        private void SelectNextUW()
        {
            var nextUW = AvailableUWs.FirstOrDefault();
            if (nextUW is not null)
            {
                selectBox_UWName = nextUW;
            }
        }

        private async Task InitPageAsync()
        {
            data = await persistenceService.TryLoadDataFromLocalStorageAsync();

            // Update AvailableUWs based on loaded data
            var usedUWs = data.Uws.Select(uw => uw.Name).ToList();
            AvailableUWs.RemoveAll(usedUWs.Contains);

            await RecalculateUWs(save: false); // Don't save during initialization
        }

        private async Task AddUW(string selectedUW)
        {
            AvailableUWs.Remove(selectedUW);
            UltimateWeapon uw = selectedUW switch
            {
                "Golden Tower" => GoldenTower.Create(),
                "Black Hole" => BlackHole.Create(data.TowerStats),
                "Death Wave" => DeathWave.Create(/*await internalDataLookup.LoadAsync<int, decimal>("data/DeathWaveDamage.json")*/),
            };
            data.Uws.Add(uw);
            SelectNextUW();
            await RecalculateUWs();
        }

        private async Task DeleteUW(UltimateWeapon uw)
        {
            AvailableUWs.Add(uw.Name);
            data.Uws.Remove(uw);
            SelectNextUW();
            await RecalculateUWs();
        }

        private async Task RecalculateUWs(bool save = true)
        {
            foreach (var uw in data.Uws)
            {
                uw.ReEvaluate();
            }
            CycleInfo = UltimateWeaponCycling.Simulate(data.Uws.Cast<Cycleable>().ToList());
            if (save)
            {
                await persistenceService.SaveDataToLocalStorageAsync(data);
            }
        }

        private async Task ClearStoredDataAsync()
        {
            await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "stones_data");
            data = new PageData();
            AvailableUWs = new(AllUWs);
            SelectNextUW();
            await RecalculateUWs(save: false);
            StateHasChanged();
        }
    }
}