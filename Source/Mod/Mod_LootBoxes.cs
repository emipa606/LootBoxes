﻿using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod
{
    public class ModLootBoxes : Verse.Mod
    {
        public static ModSettingsLootBoxes Settings;

        public ModLootBoxes(ModContentPack content) : base(content)
        {
            Settings = GetSettings<ModSettingsLootBoxes>();
        }

        [NotNull]
        public override string SettingsCategory()
        {
            return "LootBoxes_SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            Settings.DoWindowContents(rect);
            Settings.Write();
        }
    }
}