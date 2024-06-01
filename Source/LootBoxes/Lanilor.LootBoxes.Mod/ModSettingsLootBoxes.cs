using Verse;

namespace Lanilor.LootBoxes.Mod;

public class ModSettingsLootBoxes : ModSettings
{
    public bool AllowPsychicAmplifierSpawn = true;

    public float BonusLootChance = 1.5f;

    public float ChanceForGoldL = 5f;

    public float ChanceForGoldS = 10f;

    public float ChanceForPandora = 1f;

    public float ChanceForSilverL = 15f;

    public float ChanceForSilverS = 20f;

    public float ChanceForTreasure = 25f;

    public float GoldLLootboxChanceMultiplier = 1.25f;

    public float GoldLRewardValue = 1000f;

    public float GoldSLootboxChanceMultiplier = 1f;

    public float GoldSRewardValue = 650f;

    public int SetMaxGoldL = 9;

    public int SetMaxGoldS = 4;

    public int SetMaxSilverL = 12;

    public int SetMaxSilverS = 3;

    public int SetMaxTreasure = 3;

    public int SetMinGoldL = 3;

    public int SetMinGoldS = 1;

    public int SetMinSilverL = 1;

    public int SetMinSilverS = 2;

    public int SetMinTreasure = 1;

    public float SilverLLootboxChanceMultiplier = 0.75f;

    public float SilverLRewardValue = 600f;

    public float SilverSLootboxChanceMultiplier = 0.5f;

    public float SilverSRewardValue = 300f;

    public float TreasureLootboxChanceMultiplier = 0.25f;

    public float TreasureRewardValue = 150f;

    public bool UseBonusLootChance = true;
    public bool UseIngameRewardsGenerator = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref UseIngameRewardsGenerator, "UseIngameRewardsGenerator", true, true);
        Scribe_Values.Look(ref UseBonusLootChance, "UseBonusLootChance", true, true);
        Scribe_Values.Look(ref AllowPsychicAmplifierSpawn, "AllowPsychicAmplifierSpawn", true, true);
        Scribe_Values.Look(ref BonusLootChance, "BonusLootChance", 1.5f, true);
        Scribe_Values.Look(ref ChanceForTreasure, "RewardTreasureLootboxChance", 0.25f, true);
        Scribe_Values.Look(ref ChanceForSilverS, "RewardCommonSmallLootboxChance", 0.2f, true);
        Scribe_Values.Look(ref ChanceForSilverL, "RewardCommonLargeLootboxChance", 0.15f, true);
        Scribe_Values.Look(ref ChanceForGoldS, "RewardGoldSmallLootboxChance", 0.1f, true);
        Scribe_Values.Look(ref ChanceForGoldL, "RewardGoldLargeLootboxChance", 0.05f, true);
        Scribe_Values.Look(ref ChanceForPandora, "RewardPandoraLootboxChance", 0.01f, true);
        Scribe_Values.Look(ref SetMinTreasure, "TreasureBoxMinimumDropCount", 1, true);
        Scribe_Values.Look(ref SetMaxTreasure, "TreasureBoxMaximumDropCount", 3, true);
        Scribe_Values.Look(ref TreasureLootboxChanceMultiplier, "TreasureBoxRewardLootboxChanceMultiplier", 0.25f,
            true);
        Scribe_Values.Look(ref TreasureRewardValue, "TreasureBoxRewardItemsValue", 150f, true);
        Scribe_Values.Look(ref SetMinSilverS, "CommonSmallBoxMinimumDropCount", 2, true);
        Scribe_Values.Look(ref SetMaxSilverS, "CommonSmallBoxMaximumDropCount", 3, true);
        Scribe_Values.Look(ref SilverSLootboxChanceMultiplier, "CommonSmallBoxRewardLootboxChanceMultiplier", 0.5f,
            true);
        Scribe_Values.Look(ref SilverSRewardValue, "CommonSmallBoxRewardItemsValue", 300f, true);
        Scribe_Values.Look(ref SetMinSilverL, "CommonLargeBoxMinimumDropCount", 1, true);
        Scribe_Values.Look(ref SetMaxSilverL, "CommonLargeBoxMaximumDropCount", 12, true);
        Scribe_Values.Look(ref SilverLLootboxChanceMultiplier, "CommonLargeBoxRewardLootboxChanceMultiplier", 0.75f,
            true);
        Scribe_Values.Look(ref SilverLRewardValue, "CommonLargeBoxRewardItemsValue", 600f, true);
        Scribe_Values.Look(ref SetMinGoldS, "GoldSmallBoxMinimumDropCount", 1, true);
        Scribe_Values.Look(ref SetMaxGoldS, "GoldSmallBoxMaximumDropCount", 4, true);
        Scribe_Values.Look(ref GoldSLootboxChanceMultiplier, "GoldSmallBoxRewardLootboxChanceMultiplier", 1f, true);
        Scribe_Values.Look(ref GoldSRewardValue, "GoldSmallBoxRewardItemsValue", 550f, true);
        Scribe_Values.Look(ref SetMinGoldL, "GoldLargeBoxMinimumDropCount", 3, true);
        Scribe_Values.Look(ref SetMaxGoldL, "GoldLargeBoxMaximumDropCount", 9, true);
        Scribe_Values.Look(ref GoldLLootboxChanceMultiplier, "GoldLargeBoxRewardLootboxChanceMultiplier", 1.25f, true);
        Scribe_Values.Look(ref GoldLRewardValue, "GoldLargeBoxRewardItemsValue", 1000f, true);
    }

    public void Reset()
    {
        UseIngameRewardsGenerator = true;
        UseBonusLootChance = true;
        AllowPsychicAmplifierSpawn = true;
        BonusLootChance = 1.5f;
        ChanceForTreasure = 25f;
        ChanceForSilverS = 20f;
        ChanceForSilverL = 15f;
        ChanceForGoldS = 10f;
        ChanceForGoldL = 5f;
        ChanceForPandora = 1f;
        SetMinTreasure = 1;
        SetMaxTreasure = 3;
        TreasureLootboxChanceMultiplier = 0.25f;
        TreasureRewardValue = 150f;
        SetMinSilverS = 2;
        SetMaxSilverS = 3;
        SilverSLootboxChanceMultiplier = 0.5f;
        SilverSRewardValue = 300f;
        SetMinSilverL = 1;
        SetMaxSilverL = 12;
        SilverLLootboxChanceMultiplier = 0.75f;
        SilverLRewardValue = 600f;
        SetMinGoldS = 1;
        SetMaxGoldS = 4;
        GoldSLootboxChanceMultiplier = 1f;
        GoldSRewardValue = 650f;
        SetMinGoldL = 3;
        SetMaxGoldL = 9;
        GoldLLootboxChanceMultiplier = 1.25f;
        GoldLRewardValue = 1000f;
    }
}