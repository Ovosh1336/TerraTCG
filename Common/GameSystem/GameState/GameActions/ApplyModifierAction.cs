﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class ApplyModifierAction(Card card, GamePlayer player) : IGameAction
    {
        private Zone zone;

        public bool CanAcceptZone(Zone zone) => player.Owns(zone) && !zone.IsEmpty() && player.Resources.Mana >= zone.PlacedCard.ModifyIncomingSkill(card).Cost;

        public bool AcceptZone(Zone zone)
        {
            this.zone = zone;
            return true;
        }

        public ActionLogInfo GetLogMessage() => new(card, $"{ActionText("Used")} {card.CardName} {ActionText("On")} {zone.CardName}");

        public string GetZoneTooltip(Zone zone)
        {
            return $"{ActionText("Use")} {card.CardName} {ActionText("On")} {zone.CardName}";
        }

        public string GetCantAcceptZoneTooltip(Zone zone) => player.Owns(zone) && !zone.IsEmpty() ? 
            $"{ActionText("NotEnoughMana")} {ActionText("To")} {ActionText("Use")}" : "";

        public void Complete()
        {
            var showAnimation = new ShowCardAnimation(TCGPlayer.TotalGameTime, card, zone, player == TCGPlayer.LocalGamePlayer);
            player.Game.FieldAnimation = showAnimation;
            var duration = showAnimation.Duration;
            var skill = zone.PlacedCard.ModifyIncomingSkill(card);
            if(card.CardType == CardType.ITEM)
            {
                player.Game.CurrentTurn.UsedItemCount += 1;
            }

            var modifiers = card.Modifiers.Invoke();

			foreach (var modifier in modifiers)
			{
				modifier.SourceCard = card;
			}

            zone.QueueAnimation(new IdleAnimation(zone.PlacedCard, duration: duration));
            zone.QueueAnimation(new ApplyModifierAnimation(zone.PlacedCard, modifiers[0].Texture));

            zone.PlacedCard.CardModifiers.AddRange(modifiers);
            player.Resources = player.Resources.UseResource(mana: skill.Cost);
            player.Hand.Remove(card);
            if(card.SubTypes.Contains(CardSubtype.EQUIPMENT))
            {
                GameSounds.PlaySound(GameAction.USE_EQUIPMENT);
            } else
            {
                GameSounds.PlaySound(GameAction.USE_CONSUMABLE);
            }
        }
    }
}
