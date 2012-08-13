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

    public class GameObject
    {
        public int Id { get; set; }
        public Rectangle SourceRect { get; set; }

        ArrayList cardIndexList = new ArrayList();

        public GameObject()
        {
            this.SourceRect = new Rectangle();
        }

        public GameObject(int id)
        {
            this.Id = id;
            this.SourceRect = new Rectangle(id * (IncludeConstant.FrameWidth + IncludeConstant.pixel), 0, IncludeConstant.FrameWidth, IncludeConstant.FrameHeight);
        }
        
        public GameObject(int id, int x, int y, int width, int height)
        {
            Console.WriteLine("GameObject: {0} {1} {2} {3} {4}", id, x, y, width, height);

            this.Id = id;
            this.SourceRect = new Rectangle(x, y, width, height);
        }

        public void Draw(SpriteBatch spritebatch, Texture2D texture, Rectangle destRect)
        {
            spritebatch.Draw(texture, destRect, SourceRect, Color.White);
        }
    }
}
