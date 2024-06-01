namespace Lanilor.LootBoxes.Things;

public class Reward(string defName, int min = 1, int max = 1, int rand = 1)
{
    public readonly string ItemDefName = defName;

    public readonly int MaximumDropCount = max;

    public readonly int MinimumDropCount = min;

    public readonly int RandomizeDropCountUpTo = rand;
}