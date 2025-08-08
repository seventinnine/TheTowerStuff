using Microsoft.AspNetCore.Components;

namespace Website.Components
{
    public partial class PerkToggleComponent
    {
        [Parameter]
        public bool Value { get; set; }
        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        private string _id = $"switch_{Guid.NewGuid():N}";


        // Ensure two-way binding works
        private async Task OnValueChanged(ChangeEventArgs e)
        {
            Value = (bool)e.Value!;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}