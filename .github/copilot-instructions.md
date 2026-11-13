# GitHub Copilot Instructions for Loot Boxes (Continued) Mod

Welcome to the Loot Boxes (Continued) mod! This mod adds loot boxes and Pandora's box to the game, allowing players to gamble for the ultimate treasure and challenges. The mod is based on the original mod by Lanilor and updated by Pustalorc.

## Mod Overview and Purpose

The Loot Boxes (Continued) mod enhances the game by adding various types of loot boxes and Pandora's box. Players can purchase loot boxes from traders or build a mysterious red chest to exchange gold and silver for different kinds of loot boxes. Each loot box contains a variety of items, from valuable resources to powerful weapons. Pandora's box, on the other hand, can spawn incidents that may either bless or curse the colony.

## Key Features and Systems

### Loot Boxes

- **Treasure Chest**: Contains valuable items like silver, gold, or thrumbo horns.
- **Small Common Chest**: Contains items useful at the start of the game, such as food, material, and weapons.
- **Large Common Chest**: Can contain many different items, offering a chance to get enough material to build a small base or weapons for the entire colony.
- **Small Golden Chest**: Contains rare artifacts, archotech stuff, advanced armor, or high-tech weapons.
- **Large Golden Chest**: Contains even more items than the small one, including the best and most expensive items.

### Pandora's Box

- **Incidents**: Opens Pandora's box, spawning incidents that may either bless or curse the colony. Players can choose whether to open the box or not.

### Settings

- **Switch between spawn tables and in-game rewards generator**: Players can choose whether to use the default spawn tables or the in-game rewards generator.
- **Reward frequency**: The game decides when to reward loot boxes based on a reduced reward frequency.
- **Load order**: The mod should be loaded after Harmony.

### Configuration

- **Min and max drops**: Players can configure the minimum and maximum number of drops per loot box.
- **Drop reward value**: Players can set the value of the reward to be dropped.
- **Chance for other loot-boxes to drop**: Players can set the chance for other loot boxes to drop along with the main loot box.

## Coding Patterns and Conventions

### C# Coding

- **Classes and Structures**: Use descriptive class and structure names that clearly indicate their purpose. For example, `CompUseEffectLootBoxSilverLarge` for a loot box component that handles the effects of a large silver loot box.
- **Naming Conventions**: Use PascalCase for class names, methods, and properties. For example, `LootBoxType`, `MaximumMarketRewardValue`, and `RewardTable`.
- **Documentation**: Provide XML comments for public classes, methods, and properties to help other developers understand their purpose and usage.
- **Modding API**: Use the RimWorld modding API to create and configure loot boxes and Pandora's box. Follow the API guidelines to ensure compatibility with other mods.

### XML Integration

- **Defs**: Use XML files to define game elements such as job definitions, sound definitions, and recipe definitions. For example, `Jobs_Misc.xml` for job definitions, `OpeningSound.xml` for sound definitions, and `Recipes_Misc.xml` for recipe definitions.
- **ThingCategoryDefs**: Define new thing categories for loot boxes and Pandora's box. For example, `ThingCategories.xml` for thing category definitions.
- **Compatibility**: Ensure that XML definitions are compatible with other mods by using unique package IDs and avoiding conflicts.

## Harmony Patching

- **Patching**: Use Harmony patching to modify existing game behavior. For example, patching the game to allow loot boxes and Pandora's box to be spawned from quests, loot stashes, and traders.
- **Patch Names**: Use descriptive patch names to indicate their purpose. For example, `Patch_OpenLootBox` for a patch that modifies the behavior of opening a loot box.
- **Patch Location**: Place patch files in the appropriate location within the mod directory. For example, `LootBoxes\Patches\OpenLootBox.cs` for a patch that modifies the behavior of opening a loot box.
- **Patch Order**: Ensure that patches are applied in the correct order to avoid conflicts with other patches.

## Suggestions for Copilot

### C# Suggestions

- **Class Templates**: Provide class templates for new loot box components and Pandora's box components. For example, a template for `CompUseEffectLootBoxSilverLarge` that includes the necessary fields and methods.
- **Method Templates**: Provide method templates for new loot box and Pandora's box behaviors. For example, a template for `OpenBox` that includes the necessary logic for opening a Pandora's box.
- **Code Snippets**: Provide code snippets for common tasks such as configuring loot box drop rates and setting Pandora's box incident chances.
- **Documentation Snippets**: Provide XML comment snippets for public classes, methods, and properties to help developers provide clear documentation.

### XML Suggestions

- **Def Templates**: Provide XML templates for new job definitions, sound definitions, and recipe definitions. For example, a template for a new job definition that includes the necessary fields and attributes.
- **ThingCategoryDef Templates**: Provide XML templates for new thing category definitions. For example, a template for a new thing category definition that includes the necessary fields and attributes.
- **Compatibility Snippets**: Provide XML snippets for ensuring compatibility with other mods. For example, a snippet for using unique package IDs and avoiding conflicts with other mods.

### Harmony Patching Suggestions

- **Patch Templates**: Provide patch templates for new behaviors. For example, a template for a patch that modifies the behavior of opening a loot box.
- **Patch Location Snippets**: Provide snippets for placing patch files in the appropriate location within the mod directory. For example, a snippet for placing a patch file in `LootBoxes\Patches\OpenLootBox.cs`.
- **Patch Order Snippets**: Provide snippets for ensuring that patches are applied in the correct order to avoid conflicts with other patches.

By following these coding patterns, conventions, and suggestions, you can ensure that your contributions to the Loot Boxes (Continued) mod are consistent, maintainable, and compatible with other mods. Happy modding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

