using Microsoft.AspNetCore.Components;
using UltimateWeapons;

namespace Website.Components
{
    public partial class ScalingValueComponent
    {
        [Parameter]
        public ScalingValue Value { get; set; } = default!;
        [Parameter]
        public decimal SubModuleValue { get; set; } = default!;

        [Parameter]
        public EventCallback ValueChanged { get; set; }

        [Parameter]
        public EventCallback<decimal> SubModuleValueChanged { get; set; }

        private bool _isIncrementDisabled => Value.Level >= Value.MaxLevel;
        private bool _isDecrementDisabled => Value.Level <= 0;

        private async Task IncrementLevel()
        {
            if (Value.Level < Value.MaxLevel)
            {
                Value.Level++;
                await ValueChanged.InvokeAsync(Value); // Notify parent
            }
        }

        private async Task DecrementLevel()
        {
            if (Value.Level > 0)
            {
                Value.Level--;
                await ValueChanged.InvokeAsync(); // Notify parent
            }
        }

        private async Task OnSubModuleValueChanged()
        {
            await SubModuleValueChanged.InvokeAsync(SubModuleValue);
        }
    }
}