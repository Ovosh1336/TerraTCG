﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerraTCG.Common.GameSystem.CardData;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState.GameActions;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal class GamePlayer
    {
        internal Game Game { get; set; } // back reference to the game this player belongs to
        internal CardCollection Hand { get; set; }
        internal CardCollection Deck { get; set; }

        internal Field Field { get; set; }

        internal Card SelectedHandCard { get; set; }

        internal Zone SelectedFieldZone { get; set; }
        public GamePlayer Opponent => Game.GamePlayers.Find(p => p != this);

        internal IGameAction InProgressAction { get; set; }


        public GamePlayer()
        {
            Hand = new CardCollection()
            {
                Cards = [Zombie.Instance.CreateCard(), Bunny.Instance.CreateCard(), DemonEye.Instance.CreateCard()]
            };

            Deck = new CardCollection()
            {
                Cards = [Zombie.Instance.CreateCard(), Zombie.Instance.CreateCard(), Zombie.Instance.CreateCard()]
            };

            Field = new();
        }

        public void SelectZone(Zone zone)
        {
            SelectedHandCard = null;
            // TODO determine action start based on click more elegantly
            if(InProgressAction?.CanAcceptZone(zone) ?? false)
            {
                SelectedFieldZone = zone;
                var done = InProgressAction.AcceptZone(zone);
                if(done)
                {
                    InProgressAction.Complete();
                    InProgressAction = null;
                    SelectedFieldZone = null;
                }
            } else if(Owns(zone) && !zone.IsEmpty())
            {
                // Cancel any previous action
                InProgressAction?.Cancel();

                SelectedFieldZone = zone;
                InProgressAction = new MoveCardOrAttackAction(zone, this);
            }
        }

        public void SelectCardInHand(Card card)
        {
            SelectedFieldZone = null;

            SelectedHandCard = card;
            // Cancel the previous action
            InProgressAction?.Cancel();
            InProgressAction = new DeployCardAction(card, this);
        }

        internal bool Owns(Zone zone) => Field.Zones.Contains(zone);
    }
}
