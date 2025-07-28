
namespace UltimateWeapons.Cycleables.Bots;

public class GoldenBot(BotProperties properties) : Cycleable
{
    // Duration - Cooldown - Bonus - Size
    public BotProperties Properties { get; } = properties;

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateGBCooldown(), EvaluateGBDuration(), EvaluateGBBonus(), 0.06m);
    }

    private int EvaluateGBCooldown()
    {
        return (int)Properties.Slot2.CurrentValue;
    }

    private int EvaluateGBDuration()
    {
        return (int)Properties.Slot1.CurrentValue;
    }

    private decimal EvaluateGBBonus()
    {
        return Properties.Slot3.CurrentValue;
    }
}
