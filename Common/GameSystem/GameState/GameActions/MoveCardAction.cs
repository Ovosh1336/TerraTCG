﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.Drawing.Animations.FieldAnimations;
using static TerraTCG.Common.GameSystem.GameState.GameActions.IGameAction;

namespace TerraTCG.Common.GameSystem.GameState.GameActions
{
    internal class MoveCardAction(Card card, GamePlayer player) : TownsfolkAction(card, player)
    {
        private Zone sourceZone;
        private Zone destZone;

        public override ActionLogInfo GetLogMessage() => new(Card, $"{ActionText("Moved")} {destZone.CardName}");

        private int Step => sourceZone == null ? 0 : 1;

		public override bool CanAcceptZone(Zone zone) => base.CanAcceptZone(zone) &&
			(Step == 0 ? 
			!zone.IsEmpty()  && zone.Siblings.Any(z=>z.IsEmpty() && z.Index / 3 == zone.Index / 3):
			zone.Owner == sourceZone.Owner && zone.IsEmpty() && zone.Index / 3 == sourceZone.Index / 3);


        public override bool AcceptZone(Zone zone)
        {
            if(Step == 0)
            {
                sourceZone = zone;
            } else
            {
                destZone = zone;
            }
            return sourceZone != null && destZone != null;
        }

		public override string GetZoneTooltip(Zone zone)
		{
			return base.GetZoneTooltip(Step == 0 ? zone : sourceZone);
		}
		public override Zone TargetZone() => sourceZone;

        public override void Complete()
        {
            base.Complete();
            var duration = GetAnimationStartDelay();

            destZone.PlacedCard = sourceZone.PlacedCard;
            sourceZone.PlacedCard = null;

            var movedCard = destZone.PlacedCard;

            sourceZone.QueueAnimation(new IdleAnimation(movedCard, duration));
            sourceZone.QueueAnimation(new RemoveCardAnimation(movedCard));

            destZone.QueueAnimation(new NoOpAnimation(duration));
            destZone.QueueAnimation(new PlaceCardAnimation(movedCard));
            GameSounds.PlaySound(GameAction.PLACE_CARD);
        }
    }
}
