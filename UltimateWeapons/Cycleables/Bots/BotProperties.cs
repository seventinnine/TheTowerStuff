namespace UltimateWeapons.Cycleables.Bots;

public class BotProperties(ScalingValue slot1, ScalingValue slot2, ScalingValue slot3, ScalingValue slot4)
{
    public ScalingValue Slot1 { get; } = slot1;
    public ScalingValue Slot2 { get; } = slot2;
    public ScalingValue Slot3 { get; } = slot3;
    public ScalingValue Slot4 { get; } = slot4;
}
