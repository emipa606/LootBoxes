# GitHub Copilot Instructions for RimWorld Loot Boxes Mod

## Mod Overview and Purpose

The "Loot Boxes Mod" for RimWorld is designed to add a new layer of excitement and unpredictability to the game by introducing loot boxes as collectible items. These loot boxes can be opened by players to receive various rewards, each with different rarities and values. The mod aims to enhance gameplay by providing additional resource management challenges and potential high rewards for players willing to take the risk.

## Key Features and Systems

- **Loot Box Varieties**: Various types of loot boxes are available, including gold, silver, and special items like the Pandora box and treasure box. Each type offers a unique set of potential rewards.
- **Randomized Rewards**: Rewards from loot boxes are randomly determined with weighted probabilities, ensuring diverse outcomes each time a loot box is opened.
- **Reward System**: Use of `LootboxReward` and `Reward` classes to define and manage reward items, their quantities, and their drop chances.

## Coding Patterns and Conventions

- **Class Structure**: Each loot box type is represented by its class inheriting from an abstract base class `CompUseEffectLootBox`.
- **Method Overriding**: Utilization of method overriding in derived classes for specific loot box behaviors.
- **Static Definitions**: Usage of a static class `LootboxDefOf` for defining constant references to loot box types and associated data.
- **Event-driven Design**: Methods in classes are designed to interact with the RimWorld event system for effects upon item use.

## XML Integration

XML files should be used for defining loot box items and rewards within the game's asset database. This allows easy tuning of box contents, probabilities, and item definitions without altering the C# code directly.

- Define loot box items using `.xml` files to use with the `ThingSetMaker_MapGen_AncientPodContents_GiveRandomLootInventoryForTombPawn` class.
- Ensure proper XML tags and structures that align with RimWorld's modding schema, such as `<ThingDefs>` to define new items or `<RewardDefs>` for reward specifications.

## Harmony Patching

Harmony is used for patching existing RimWorld classes to integrate the loot box functionalities seamlessly.

- Utilize Harmony to patch and modify the base game method behaviors where necessary, ensuring non-intrusive integration of new features.
- Common uses could include modifying the odds of items in existing loot tables or changing how items are consumed in the game.

## Suggestions for Copilot

- **Boilerplate Code Generation**: Use Copilot to generate boilerplate class and method definitions for quick implementation of new loot box types.
- **Pattern Recognition**: Copilot can assist in identifying and applying common patterns, such as Factory patterns for generating different loot boxes or Strategy patterns for handling various loot outcomes.
- **XML Template Creation**: Generate XML templates for defining new loot box items and rewards, ensuring adherence to RimWorld's mod XML schema.
- **Harmony Patch Examples**: Suggest example Harmony patches that target specific methods relevant to loot box logic, facilitating easy enhancements or bug fixes.

By leveraging the capabilities of GitHub Copilot, you can expedite the development process of this mod, ensuring efficient coding practices and seamless integration with existing game systems.
