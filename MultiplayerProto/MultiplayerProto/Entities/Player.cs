using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Entities
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using MultiplayerProto.Constants;
    using MultiplayerProto.Managers;

    public class Player
    {
        //private long id;
        public long Id { get; set; }

        //private Texture2D texture;
        public Texture2D Texture { get; set; }

        //private int cardIndex;
        public int CardIndex { get; set; }

        Rectangle playerCardPosition;
        Rectangle sourceRect;

        private Vector2 mouseClickPosition;
        private Rectangle pixelRect; // To see if click position intersects with bounding rectangles

        InputManager inputManager;
        CardManager cardManager;

        internal Player(long id, Texture2D texture, InputManager inputmanager, CardManager cardManager)
        {
            this.Id = id;
            this.Texture = texture;
            this.inputManager = inputmanager;
            this.cardManager = cardManager;

            this.CardIndex = 0; // set to blank initially

            playerCardPosition = new Rectangle(10, 10, 190, 254);

            mouseClickPosition.X = 0;
            mouseClickPosition.Y = 0;
        }

        public void Draw(SpriteBatch spritebatch, Rectangle destRect)
        {
            // TO-DO: Player draw code goes here
            
            ////spriteBatch.Draw(Texture, playerCardPosition, Color.White);
            //// Draw Hello World
            //string output = "Mouse Position: " + lastClick.X + "," + lastClick.Y ;

            //// Find the center of the string
            //Vector2 FontOrigin = font.MeasureString(output) / 2;
            //// Draw the string
            //spriteBatch.DrawString(font, output, fontPosition, Color.LightGreen,
            //    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            // Each player draws its card on the screen
            sourceRect = new Rectangle(this.CardIndex * (IncludeConstant.FrameWidth + IncludeConstant.pixel), 0, IncludeConstant.FrameWidth, IncludeConstant.FrameHeight);

            spritebatch.Draw(this.Texture, destRect, sourceRect, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            // Player based updates go here
            if (this.inputManager.isLeftButtonClicked())
            {
                Console.WriteLine("TEST >> My id is: {0}", this.Id);

                this.mouseClickPosition = this.inputManager.MousePosition;
                pixelRect = new Rectangle((int)mouseClickPosition.X, (int)mouseClickPosition.Y, 1, 1);

                if (IncludeConstant.boundingRect_card1.Contains(pixelRect))
                {
                    Console.WriteLine("Click detected on Card 1");
                    this.CardIndex = this.cardManager.getCardIndex(0);
                }

                if (IncludeConstant.boundingRect_card2.Contains(pixelRect))
                {
                    Console.WriteLine("Click detected on Card 2");
                    this.CardIndex = this.cardManager.getCardIndex(1);
                }

                if (IncludeConstant.boundingRect_card3.Contains(pixelRect))
                {
                    Console.WriteLine("Click detected on Card 3");
                    this.CardIndex = this.cardManager.getCardIndex(2);
                }

                if (IncludeConstant.boundingRect_card4.Contains(pixelRect))
                {
                    Console.WriteLine("Click detected on Card 4");
                    this.CardIndex = this.cardManager.getCardIndex(3);
                }
            }
        }        
    }
}
