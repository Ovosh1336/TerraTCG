﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TerraTCG.Common.GameSystem.BotPlayer;
using TerraTCG.Common.GameSystem.CardData;
using TerraTCG.Common.GameSystem.Drawing.Animations;
using TerraTCG.Common.GameSystem.GameState.GameActions;

namespace TerraTCG.Common.GameSystem.GameState
{
    internal class GamePlayer
    {
        internal const int MAX_HEALTH = 3;
        internal const int MAX_MANA = 8;
        internal CardGame Game { get; set; } // back reference to the game this player belongs to
        internal CardCollection Hand { get; set; }
        internal CardCollection Deck { get; set; }
        internal int ManaPerTurn { get; set; } = 0;

        public PlayerResources _resources = new(3, 0, 0);
        public PlayerResources Resources { 
            get => _resources; 
            set
            {
                PrevResources = _resources;
                _resources = value;
            }
        }
        public PlayerResources PrevResources { get; private set; }

        internal Field Field { get; set; }

        internal int SelectedHandIdx { get; set; }
        internal Card SelectedHandCard { get; set; }

        internal Zone SelectedFieldZone { get; set; }
        public GamePlayer Opponent => Game.GamePlayers.Find(p => p != this);

        internal IGameAction InProgressAction { get; set; }

        // TODO real implementation
        internal bool IsMyTurn => Game.CurrentTurn.ActivePlayer == this;

        public static Card GetCard<T>() where T : BaseCardTemplate, ICardTemplate
        {
            return ModContent.GetInstance<T>().Card;
        }

        public GamePlayer(CardGame game, CardCollection deck)
        {
            Game = game;

            Deck = deck;
            Deck.Shuffle();
        
            // Auto-mulligan a 1+ creature hand for the player
            while(!Deck.Cards.Take(5).Any(c=>c.CardType == CardType.CREATURE && c.SubTypes[0] != CardSubtype.EXPERT))
            {
                Deck.Shuffle();
            }

            Hand = new CardCollection()
            {
                Cards = [ ]
            };

            for (int _ = 0; _ < 5; _++)
            {
                Hand.Add(Deck.Draw());
            }
            Field = new(game);
        }

        public void SelectZone(Zone zone)
        {
            SelectedHandCard = null;
            SelectedHandIdx = -1;
            // TODO determine action start based on click more elegantly
            if(IsMyTurn && (InProgressAction?.CanAcceptZone(zone) ?? false))
            {
                SelectedFieldZone = zone;
                var done = InProgressAction.AcceptZone(zone);
                if(done)
                {
                    InProgressAction.Complete();
                    InProgressAction = null;
                    SelectedFieldZone = null;
                }
            } else if(IsMyTurn && Owns(zone) && !zone.IsEmpty())
            {
                // Cancel any previous action
                InProgressAction?.Cancel();

                SelectedFieldZone = zone;
                InProgressAction = zone.PlacedCard?.Template.SelectOnFieldAction(zone, this);
            }
        }

        public void SelectCardInHand(int handIdx)
        {
            SelectedFieldZone = null;

            SelectedHandIdx = handIdx;
            SelectedHandCard = Hand.Cards[handIdx];
            if(IsMyTurn)
            {
                // Cancel the previous action
                InProgressAction?.Cancel();
                InProgressAction = SelectedHandCard?.SelectInHandAction(SelectedHandCard, this);
            }
        }

        public void SelectActionButton(ActionType actionType)
        {
            if(InProgressAction?.CanAcceptActionButton() ?? false)
            {
                var done = InProgressAction.AcceptActionButton();
                if(done)
                {
                    InProgressAction.Complete();
                    SelectedFieldZone = null;
                    InProgressAction = null;
                }
            }
        }

        public void PassTurn()
        {
            // TODO real implementation
            if(IsMyTurn)
            {
                Game.CurrentTurn.End();
            }
        }

        internal bool Owns(Zone zone) => Field.Zones.Contains(zone);
    }
}
