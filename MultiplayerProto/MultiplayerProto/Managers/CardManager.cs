using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Managers
{
    using System.Collections;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using MultiplayerProto.Entities;
    using MultiplayerProto.Managers;
    using MultiplayerProto.Constants;
    using MultiplayerProto.RandomNumbers;

    public class CardManager
    {
        private ArrayList cardsInHand;        
        
        private Texture2D spriteSheet;

        Rectangle _destRect;

        private IRandomNumberGenerator randomNumberGenerator = NullRandomNumberGenerator.Instance;

        public CardManager(IRandomNumberGenerator randomNumberGenerator)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            
            this.cardsInHand = new ArrayList();

            // Initialize first pile of cards in hand
            for (int i = 0; i < IncludeConstant.InitCardsInHand; i++)
            {
                cardsInHand.Add(getCardFromPile());
            }

            // Hard coding cards for test purposes
            //cardsInHand.Add(new Card(1));
            //cardsInHand.Add(new Card(2));
            //cardsInHand.Add(new Card(3));
            //cardsInHand.Add(new Card(4));
        }

        // TO-DO: Return distinct cards from pile (no cards that already exist)
        public Card getCardFromPile()
        {
            // Return random card from pile
            int randomCardIndex = this.randomNumberGenerator.Next(4);
            Console.WriteLine("Card {0}", randomCardIndex);

            return new Card(randomCardIndex);
        }

        public void LoadContent(ContentManager contentManager)
        {
            this.spriteSheet = contentManager.Load<Texture2D>(@"Textures\SpriteSheet");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw local cards here
            int i = 0;
            foreach (Card card in cardsInHand)
            {
                //_destRect = new Rectangle(IncludeConstant.leftMargin + (card.Id * IncludeConstant.FrameWidth), IncludeConstant.localCardPosition_Y,
                //    IncludeConstant.FrameWidth - IncludeConstant.pixel, IncludeConstant.FrameHeight);

                // Get card position from IncludeConstants
                _destRect = new Rectangle(IncludeConstant.leftMargin + IncludeConstant.localCardPositions[i], IncludeConstant.localCardPosition_Y,
                    IncludeConstant.FrameWidth - IncludeConstant.pixel, IncludeConstant.FrameHeight);
                                
                card.Draw(spriteBatch, spriteSheet, _destRect);

                i++;
            }
        }

        public int getCardIndex(int cardPosition)
        {
            if (cardPosition >= this.cardsInHand.Count)
            {
                return 0;
            }
            else
            {
                Card card = (Card)this.cardsInHand[cardPosition];
                return card.Id;
            }
        }
    }
}
