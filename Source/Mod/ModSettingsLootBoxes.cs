using Verse;

namespace Lanilor.LootBoxes.Mod
{
    public class ModSettingsLootBoxes : ModSettings
    {
        public bool UseIngameRewardsGenerator = true;
        public bool UseBonusLootChance = true;
#if !V10
        public bool AllowPsychicAmplifierSpawn = true;
#endif

        public float BonusLootChance = 1.5f;

        public float ChanceForTreasure = 25;
        public float ChanceForSilverS = 20;
        public float ChanceForSilverL = 15;
        public float ChanceForGoldS = 10;
        public float ChanceForGoldL = 5;
        public float ChanceForPandora = 1;

        public int SetMinTreasure = 1;
        public int SetMaxTreasure = 3;
        public float TreasureLootboxChanceMultiplier = 0.25f;
        public float TreasureRewardValue = 150;

        public int SetMinSilverS = 2;
        public int SetMaxSilverS = 3;
        public float SilverSLootboxChanceMultiplier = 0.5f;
        public float SilverSRewardValue = 300;

        public int SetMinSilverL = 1;
        public int SetMaxSilverL = 12;
        public float SilverLLootboxChanceMultiplier = 0.75f;
        public float SilverLRewardValue = 600;

        public int SetMinGoldS = 1;
        public int SetMaxGoldS = 4;
        public float GoldSLootboxChanceMultiplier = 1;
        public float GoldSRewardValue = 650;

        public int SetMinGoldL = 3;
        public int SetMaxGoldL = 9;
        public float GoldLLootboxChanceMultiplier = 1.25f;
        public float GoldLRewardValue = 1000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref UseIngameRewardsGenerator, "UseIngameRewardsGenerator", true, true);
            Scribe_Values.Look(ref UseBonusLootChance, "UseBonusLootChance", true, true);
#if !V10
            Scribe_Values.Look(ref AllowPsychicAmplifierSpawn, "AllowPsychicAmplifierSpawn", true, true);
#endif
            Scribe_Values.Look(ref BonusLootChance, "BonusLootChance", 1.5f, true);
            Scribe_Values.Look(ref ChanceForTreasure, "RewardTreasureLootboxChance", 0.25f, true);
            Scribe_Values.Look(ref ChanceForSilverS, "RewardCommonSmallLootboxChance", 0.20f, true);
            Scribe_Values.Look(ref ChanceForSilverL, "RewardCommonLargeLootboxChance", 0.15f, true);
            Scribe_Values.Look(ref ChanceForGoldS, "RewardGoldSmallLootboxChance", 0.10f, true);
            Scribe_Values.Look(ref ChanceForGoldL, "RewardGoldLargeLootboxChance", 0.05f, true);
            Scribe_Values.Look(ref ChanceForPandora, "RewardPandoraLootboxChance", 0.01f, true);
            Scribe_Values.Look(ref SetMinTreasure, "TreasureBoxMinimumDropCount", 1, true);
            Scribe_Values.Look(ref SetMaxTreasure, "TreasureBoxMaximumDropCount", 3, true);
            Scribe_Values.Look(ref TreasureLootboxChanceMultiplier, "TreasureBoxRewardLootboxChanceMultiplier", 0.25f,
                true);
            Scribe_Values.Look(ref TreasureRewardValue, "TreasureBoxRewardItemsValue", 150, true);
            Scribe_Values.Look(ref SetMinSilverS, "CommonSmallBoxMinimumDropCount", 2, true);
            Scribe_Values.Look(ref SetMaxSilverS, "CommonSmallBoxMaximumDropCount", 3, true);
            Scribe_Values.Look(ref SilverSLootboxChanceMultiplier, "CommonSmallBoxRewardLootboxChanceMultiplier", 0.5f,
                true);
            Scribe_Values.Look(ref SilverSRewardValue, "CommonSmallBoxRewardItemsValue", 300, true);
            Scribe_Values.Look(ref SetMinSilverL, "CommonLargeBoxMinimumDropCount", 1, true);
            Scribe_Values.Look(ref SetMaxSilverL, "CommonLargeBoxMaximumDropCount", 12, true);
            Scribe_Values.Look(ref SilverLLootboxChanceMultiplier, "CommonLargeBoxRewardLootboxChanceMultiplier", 0.75f,
                true);
            Scribe_Values.Look(ref SilverLRewardValue, "CommonLargeBoxRewardItemsValue", 600, true);
            Scribe_Values.Look(ref SetMinGoldS, "GoldSmallBoxMinimumDropCount", 1, true);
            Scribe_Values.Look(ref SetMaxGoldS, "GoldSmallBoxMaximumDropCount", 4, true);
            Scribe_Values.Look(ref GoldSLootboxChanceMultiplier, "GoldSmallBoxRewardLootboxChanceMultiplier", 1, true);
            Scribe_Values.Look(ref GoldSRewardValue, "GoldSmallBoxRewardItemsValue", 550, true);
            Scribe_Values.Look(ref SetMinGoldL, "GoldLargeBoxMinimumDropCount", 3, true);
            Scribe_Values.Look(ref SetMaxGoldL, "GoldLargeBoxMaximumDropCount", 9, true);
            Scribe_Values.Look(ref GoldLLootboxChanceMultiplier, "GoldLargeBoxRewardLootboxChanceMultiplier", 1.25f,
                true);
            Scribe_Values.Look(ref GoldLRewardValue, "GoldLargeBoxRewardItemsValue", 1000, true);
        }

        public void Reset()
        {
            UseIngameRewardsGenerator = true;
            UseBonusLootChance = true;
#if !V10
            AllowPsychicAmplifierSpawn = true;
#endif
            BonusLootChance = 1.5f;

            ChanceForTreasure = 25;
            ChanceForSilverS = 20;
            ChanceForSilverL = 15;
            ChanceForGoldS = 10;
            ChanceForGoldL = 5;
            ChanceForPandora = 1;

            SetMinTreasure = 1;
            SetMaxTreasure = 3;
            TreasureLootboxChanceMultiplier = 0.25f;
            TreasureRewardValue = 150;

            SetMinSilverS = 2;
            SetMaxSilverS = 3;
            SilverSLootboxChanceMultiplier = 0.5f;
            SilverSRewardValue = 300;

            SetMinSilverL = 1;
            SetMaxSilverL = 12;
            SilverLLootboxChanceMultiplier = 0.75f;
            SilverLRewardValue = 600;

            SetMinGoldS = 1;
            SetMaxGoldS = 4;
            GoldSLootboxChanceMultiplier = 1;
            GoldSRewardValue = 650;

            SetMinGoldL = 3;
            SetMaxGoldL = 9;
            GoldLLootboxChanceMultiplier = 1.25f;
            GoldLRewardValue = 1000;
        }
    }
}