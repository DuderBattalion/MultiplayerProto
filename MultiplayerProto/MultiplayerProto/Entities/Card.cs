using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Entities
{
     using System.Collections;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using MultiplayerProto.Constants;

    public class Card
    {
        public int Id { get; set; }
        public Rectangle SourceRect { get; set; }

        ArrayList cardIndexList = new ArrayList();

        public Card()
        {
            this.SourceRect = new Rectangle();
        }

        public Card(int id)
        {
            this.Id = id;
            this.SourceRect = new Rectangle(id * (IncludeConstant.FrameWidth + IncludeConstant.pixel), 0, IncludeConstant.FrameWidth, IncludeConstant.FrameHeight);
        }
        
        public void Draw(SpriteBatch spritebatch, Texture2D texture, Rectangle destRect)
        {
            spritebatch.Draw(texture, destRect, SourceRect, Color.White);
        }
    }
}
