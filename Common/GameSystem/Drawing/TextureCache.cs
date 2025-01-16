﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.CardData;
using TerraTCG.Common.GameSystem.GameState;
using TerraTCG.Common.GameSystem.GameState.Modifiers;

namespace TerraTCG.Common.GameSystem.Drawing
{
    internal class TextureCache : ModSystem
    {

        internal static TextureCache Instance => ModContent.GetInstance<TextureCache>();

        internal Asset<Texture2D> Field { get; private set; }
        internal Asset<Texture2D> Zone { get; private set; }
        internal Asset<Texture2D> ZoneHighlighted { get; private set; }

        internal Asset<Texture2D> ZoneSelectable { get; private set; }

        internal Asset<Texture2D> CardBack { get; private set; }

        internal Asset<Texture2D> OffenseIcon { get; private set; }
        internal Asset<Texture2D> DefenseIcon { get; private set; }
        internal Asset<Texture2D> HeartIcon { get; private set; }

        internal Asset<Texture2D> ManaIcon { get; private set; }
        public Asset<Texture2D> MoveIcon { get; private set; }
        public Asset<Texture2D> Button { get; private set; }
        public Asset<Texture2D> ButtonHighlighted { get; private set; }
        public Asset<Texture2D> StarIcon { get; private set; }
        public Asset<Texture2D> TownsfolkIcon { get; private set; }
        public Asset<Texture2D> PlayerStatsZone { get; private set; }
        public Asset<Texture2D> AttackIcon { get; private set; }
        public Asset<Texture2D> LightRay { get; private set; }
        public Asset<Texture2D> MapBG { get; private set; }
        public Asset<Texture2D> CancelButton { get; private set; }
        public Asset<Texture2D> CardPreviewFrame { get; private set; }
        public Asset<Texture2D> BiomeIcons { get; private set; }
        public Asset<Texture2D> EmoteIcons { get; private set; }
		public Asset<Texture2D> Foiling { get; private set; }
		public Asset<Texture2D> Sparkles { get; private set; }
		public Asset<Texture2D> Sparkles2 { get; private set; }
		public Asset<Texture2D> KingSlimeCrown { get; private set; }
        internal Dictionary<int, Asset<Texture2D>> BestiaryTextureCache { get; private set; }
        internal Dictionary<int, Asset<Texture2D>> NPCTextureCache { get; private set; }
        internal Dictionary<int, Asset<Texture2D>> ItemTextureCache { get; private set; }

        internal Dictionary<ModifierType, Asset<Texture2D>> ModifierIconTextures { get; private set; }
		public Dictionary<CardSubtype, Asset<Texture2D>> FoilMasks { get; private set; }
		public Dictionary<string, Asset<Texture2D>> CardFoilMasks { get; private set; }
		public Dictionary<CardSubtype, Asset<Texture2D>> BiomeMapBackgrounds { get; private set; }
        internal Dictionary<CardSubtype, Rectangle> BiomeIconBounds { get; private set; }
        internal Dictionary<CardSubtype, Rectangle> CardTypeEmoteBounds { get; private set; }

        internal const int TUTORIAL_SLIDE_COUNT = 18;

        public Asset<Texture2D> TutorialFrame { get; private set; }
        internal List<Asset<Texture2D>> TutorialSlides { get; private set; }
        internal List<Asset<Texture2D>> TutorialOverlays { get; private set; }
        public override void Load()
        {
            base.Load();
            Field = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Field");
            Zone = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone");
            ZoneHighlighted = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone_Highlighted");
            ZoneSelectable = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Zone_Selectable");
            CardBack = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Card_Back");
            OffenseIcon = Main.Assets.Request<Texture2D>("Images/UI/PVP_0");
            DefenseIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.CobaltShield);
            HeartIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.Heart);
            ManaIcon = Main.Assets.Request<Texture2D>("Images/Item_" + ItemID.Star);
            MoveIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Boots_Icon");
            Button = Mod.Assets.Request<Texture2D>("Assets/FieldElements/RadialButton");
            ButtonHighlighted = Main.Assets.Request<Texture2D>("Images/UI/Wires_1");
            StarIcon = Main.Assets.Request<Texture2D>("Images/Projectile_" + ProjectileID.FallingStar);
            TownsfolkIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/TownsfolkMana");
            PlayerStatsZone = Mod.Assets.Request<Texture2D>("Assets/FieldElements/PlayerStats");
            AttackIcon = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Attack_Icon");
            LightRay = Main.Assets.Request<Texture2D>("Images/Projectile_" + ProjectileID.MedusaHeadRay);
            CancelButton = Mod.Assets.Request<Texture2D>("Assets/FieldElements/CancelGame");
            CardPreviewFrame = Mod.Assets.Request<Texture2D>("Assets/FieldElements/CardPreviewFrame");
            BiomeIcons = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");
            EmoteIcons = Main.Assets.Request<Texture2D>("Images/Extra_"+ExtrasID.EmoteBubble);
            Foiling = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Foil");
            Sparkles = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Sparkles");
            Sparkles2 = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/Sparkles2");

            KingSlimeCrown = Main.Assets.Request<Texture2D>("Images/Extra_" + ExtrasID.KingSlimeCrown);
            NPCTextureCache = [];
			NPCTextureCache[NPCID.EaterofWorldsHead] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/MiniEoW");
            BestiaryTextureCache = [];
            ItemTextureCache = [];
            ModifierIconTextures = new Dictionary<ModifierType, Asset<Texture2D>>
            {
                [ModifierType.PAUSED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/PausedIcon"),
                [ModifierType.SPIKED] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Spiked_Icon"),
                [ModifierType.DEFENSE_BOOST] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Defense_Icon"),
                [ModifierType.EVASIVE] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Evasive_Icon"),
                [ModifierType.RELENTLESS] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Relentless_Icon"),
                [ModifierType.BLEEDING] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Bleed_Icon"),
                [ModifierType.POISON] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Poison_Icon"),
                [ModifierType.MORBID] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Morbid_Icon"),
                [ModifierType.LIFESTEAL] = Mod.Assets.Request<Texture2D>("Assets/FieldElements/Lifesteal_Icon"),
            };

            FoilMasks = new Dictionary<CardSubtype, Asset<Texture2D>>
            {
                [CardSubtype.FOREST] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/FOREST"),
                [CardSubtype.CAVERN] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CAVERN"),
                [CardSubtype.JUNGLE] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/JUNGLE"),
                [CardSubtype.GOBLIN_ARMY] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/GOBLIN_ARMY"),
                [CardSubtype.BLOOD_MOON] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/BLOOD_MOON"),
                [CardSubtype.OCEAN] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/OCEAN"),
                [CardSubtype.MUSHROOM] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/MUSHROOM"),
                [CardSubtype.CRIMSON] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/CRIMSON"),
                [CardSubtype.CONSUMABLE] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/ITEM"),
                [CardSubtype.EQUIPMENT] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/ITEM"),
                [CardSubtype.TOWNSFOLK] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/TOWNSFOLK"),
            };

			CardFoilMasks = new Dictionary<string, Asset<Texture2D>>
			{
				[ModContent.GetInstance<KingSlime>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/KingSlime"),
				[ModContent.GetInstance<QueenBee>().Card.FullName] = Mod.Assets.Request<Texture2D>("Assets/FoilMasks/QueenBee"),
			};

            BiomeMapBackgrounds = new Dictionary<CardSubtype, Asset<Texture2D>>
            {
                [CardSubtype.FOREST] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
                [CardSubtype.CAVERN] = Main.Assets.Request<Texture2D>("Images/MapBG2"),
                [CardSubtype.JUNGLE] = Main.Assets.Request<Texture2D>("Images/MapBG9"),
                [CardSubtype.GOBLIN_ARMY] = Main.Assets.Request<Texture2D>("Images/MapBG1"),
                [CardSubtype.BLOOD_MOON] = Main.Assets.Request<Texture2D>("Images/MapBG26"),
                [CardSubtype.OCEAN] = Main.Assets.Request<Texture2D>("Images/MapBG11"),
                [CardSubtype.MUSHROOM] = Main.Assets.Request<Texture2D>("Images/MapBG20"),
                [CardSubtype.CRIMSON] = Main.Assets.Request<Texture2D>("Images/MapBG26"),
            };

            BiomeIconBounds = new Dictionary<CardSubtype, Rectangle>
            {
                [CardSubtype.FOREST] = new Rectangle(0, 0, 30, 30),
                [CardSubtype.CAVERN] = new Rectangle(60, 0, 30, 30),
                [CardSubtype.JUNGLE] = new Rectangle(180, 30, 30, 30),
                [CardSubtype.GOBLIN_ARMY] = new Rectangle(30, 90, 30, 30),
                [CardSubtype.BLOOD_MOON] = new Rectangle(180, 60, 30, 30),
                [CardSubtype.OCEAN] = new Rectangle(360, 30, 30, 30),
                [CardSubtype.MUSHROOM] = new Rectangle(240, 30, 30, 30),
                [CardSubtype.CRIMSON] = new Rectangle(360, 0, 30, 30),
            };

            CardTypeEmoteBounds = new Dictionary<CardSubtype, Rectangle>
            {
                [CardSubtype.EQUIPMENT] = new Rectangle(137, 557, 30, 30),
                [CardSubtype.CONSUMABLE] = new Rectangle(103, 527, 30, 30),
                [CardSubtype.TOWNSFOLK] = new Rectangle(69, 753, 30, 30)
            };
        }

        public Asset<Texture2D> GetNPCTexture(int npcId)
        {
            if(!NPCTextureCache.TryGetValue(npcId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/NPC_{npcId}");
                NPCTextureCache[npcId] = asset;
            }             
            return asset;
        }
        public Asset<Texture2D> GetItemTexture(int itemId)
        {
            if(!ItemTextureCache.TryGetValue(itemId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/Item_{itemId}");
                ItemTextureCache[itemId] = asset;
            }             
            return asset;
        }

        public Asset<Texture2D> GetBestiaryTexture(int npcId)
        {
            if(!ItemTextureCache.TryGetValue(npcId, out var asset))
            {
                asset = Main.Assets.Request<Texture2D>($"Images/UI/Bestiary/NPCs/NPC_{npcId}");
				BestiaryTextureCache[npcId] = asset;
            }             
            return asset;
        }

        // Tutorial images are large, don't load them if we don't need to
        public void LoadTutorial()
        {
            TutorialFrame = Mod.Assets.Request<Texture2D>($"Assets/Tutorial/TutorialFrame");
            TutorialSlides = [];
            TutorialOverlays = [];
            for(int i = 0; i < TUTORIAL_SLIDE_COUNT; i++)
            {
                TutorialSlides.Add(
                    Mod.Assets.Request<Texture2D>($"Assets/Tutorial/Tutorial{i}"));
            }
            for(int i = 0; i < 16; i++)
            {
                TutorialOverlays.Add(
                    Mod.Assets.Request<Texture2D>($"Assets/Tutorial/TutorialOverlay{i}"));
            }
        }
    }
}
