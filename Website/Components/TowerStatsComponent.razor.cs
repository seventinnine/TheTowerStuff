using Microsoft.AspNetCore.Components;
using UltimateWeapons;

namespace Website.Components
{
    public partial class TowerStatsComponent
    {
        [Parameter] public TowerStats TowerStats { get; set; } = new TowerStats(69.5m);
        [Parameter] public EventCallback ValueChanged { get; set; }

        private async Task OnValueChanged()
        {
            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync();
            }
        }
    }
}