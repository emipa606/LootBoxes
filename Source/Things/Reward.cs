namespace Lanilor.LootBoxes.Things
{
    public class Reward
    {
        public string ItemDefName;

        public int MinimumDropCount;

        public int MaximumDropCount;

        public int RandomizeDropCountUpTo;

        public Reward(string defName, int min = 1, int max = 1, int rand = 1)
        {
            ItemDefName = defName;
            MinimumDropCount = min;
            MaximumDropCount = max;
            RandomizeDropCountUpTo = rand;
        }
    }
}