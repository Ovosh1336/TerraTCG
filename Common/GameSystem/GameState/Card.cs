﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.Drawing;
using TerraTCG.Common.GameSystem.GameState.GameActions;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal enum CardType
    {
        CREATURE,
        ITEM,
        TOWNSFOLK
    }

    internal enum CardSubtype
    {
        // Empty value
        NONE,
        // Biomes
        FOREST,
        CAVERN,
        // Duplicate CardTypes (for text rendering)
        TOWNSFOLK,
        ITEM,
        // Item Subtypes
        EQUIPMENT,
        CONSUMABLE,
        // Creature Supertypes
        EXPERT,
        BOSS,
        // Creature Roles
        FIGHTER,
        SCOUT,
        DEFENDER,
        SLIME,
        CRITTER,
        CASTER,

        // Biomes
        JUNGLE,
        SKY,
        BLOOD_MOON,
        DESERT,
        OCEAN,
        GOBLIN_ARMY,
        MUSHROOM,
    }

    // Bot helper function, apply additional
    // logic for whether the bot should use
    // this item on an ally
    internal delegate bool ShouldTarget(Zone zone);

    internal class Card
    {
        internal string Name { get; set; }

        internal int NPCID { get; set; }

        internal Asset<Texture2D> Texture => ModContent.GetInstance<TerraTCG>().Assets.Request<Texture2D>($"Assets/Cards/{Name}");

        internal CardType CardType { get; set; }

        internal List<CardSubtype> SubTypes { get; set; }

        internal int MaxHealth { get; set; }

        internal int MoveCost { get; set; }

        // Points awarded on defeating this card
        internal int Points { get; set; } = 1;

        internal SelectInHandAction SelectInHandAction { get; set; }
            = (zone, player) => new DeployCreatureAction(zone, player);
        internal SelectOnFieldAction SelectOnFieldAction { get; set; }
            = (zone, player) => new MoveCardOrAttackAction(zone, player);

        internal List<Skill> Skills { get; set; }

        internal bool HasSkill => (Skills?.Count ?? 0) > 0;

        internal List<Attack> Attacks { get; set; }

        internal List<ICardModifier> Modifiers { get; set; }

        // Tags to help bot players decide what to do with cards

        // Where to place the card on the board
        internal ZoneRole Role { get; set; } = ZoneRole.OFFENSE;


        // Among cards of the same type, how important is it to play this one first?
        internal int Priority { get; set; } = 0;

        internal ShouldTarget ShouldTarget { get; set; } = z => true;


        // Localization utils
        internal string CardName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Name");

        internal string AttackName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Attack.Name");
        internal string AttackDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Attack.Description");
        internal string SkillName => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Skill.Name");
        internal string SkillDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Skill.Description");

        internal string ModifierDescription => Language.GetTextValue($"Mods.TerraTCG.Cards.{Name}.Modifier.Description");

        internal bool HasAttackText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Attack.Name");
        internal bool HasAttackDescription => HasAttackText && Language.Exists($"Mods.TerraTCG.Cards.{Name}.Attack.Description");
        internal bool HasSkillText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Skill.Name");
        internal bool HasSkillDescription => HasSkillText && Language.Exists($"Mods.TerraTCG.Cards.{Name}.Skill.Description");
        internal bool HasModifierText => Language.Exists($"Mods.TerraTCG.Cards.{Name}.Modifier.Description");

        internal string TypeLine => string.Join(" ", 
            SubTypes.Select(t => Language.GetTextValue($"Mods.TerraTCG.Cards.Types.{t}")));

        internal DrawZoneNPC DrawZoneNPC { get; set; } = CardOverlayRenderer.Instance.DefaultDrawZoneNPC;
    }
}
