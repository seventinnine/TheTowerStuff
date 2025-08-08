using UltimateWeapons;
using UltimateWeapons.Cycleables;
using UltimateWeapons.Cycleables.Weapons;

namespace Website.Pages
{
    public partial class Stones
    {
        private string selectBox_UWName;

        private TowerStats towerStats = new TowerStats(69.5m);

        private UltimateWeaponCycling simualtor = new();

        private List<UltimateWeapon> uws = new();
        private static string[] AllUWs = [
            "Golden Tower",
            "Black Hole",
            "Death Wave",
            //"Golden Bot",
            //"Spotlight"
            ];
        private List<string> AvailableUWs = [];
        private bool DisableAddingUWs => AvailableUWs.Count < 1;
        public decimal CoinsPerSecond { get; private set; }

        protected override void OnInitialized()
        {
            AvailableUWs = new(AllUWs);
            SelectNextUW();
            InitPage();
        }

        private void SelectNextUW()
        {
            var nextUW = AvailableUWs.FirstOrDefault();
            if (nextUW is not null)
            {
                selectBox_UWName = nextUW;
            }
        }

        private void InitPage()
        {
            // TODO
        }

        private void AddUW(string selectedUW)
        {
            AvailableUWs.Remove(selectedUW);
            UltimateWeapon uw = selectedUW switch
            {
                "Golden Tower" => GoldenTower.Create(),
                "Black Hole" => BlackHole.Create(towerStats),
                "Death Wave" => DeathWave.Create(),
            };
            uws.Add(uw);
            SelectNextUW();
        }

        private void RecalculateUWs()
        {
            foreach (var uw in uws)
            {
                uw.ReEvaluate();
            }
            CoinsPerSecond = UltimateWeaponCycling.Simulate(uws.Cast<Cycleable>().ToList()).CoinsPerSecond;
        }
    }
}