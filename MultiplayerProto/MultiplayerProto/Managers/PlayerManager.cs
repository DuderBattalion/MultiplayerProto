using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MultiplayerProto.RandomNumbers;
using MultiplayerProto.EventArgs;
using MultiplayerProto.Entities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiplayerProto.Managers
{
    using MultiplayerProto.Constants;

    public class PlayerManager
    {
        private IRandomNumberGenerator randomNumberGenerator = NullRandomNumberGenerator.Instance;

        private readonly InputManager inputManager;
        private CardManager cardManager;

        private readonly bool isHost;

        private readonly Dictionary<long, Player> players = new Dictionary<long, Player>();

        private Texture2D spriteSheet;

        private Player localPlayer;

        private static long playerIdCounter;

        private GameTimer hearbeatTimer;

        Rectangle _destRect;

        public PlayerManager(
            IRandomNumberGenerator randomNumberGenerator,
            InputManager inputManager,
            CardManager cardManager,
            bool isHost)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.inputManager = inputManager;
            this.cardManager = cardManager;
            this.isHost = isHost;
        }

        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players.Values;
            }
        }

        public IRandomNumberGenerator RandomNumberGenerator
        {
            get
            {
                return this.randomNumberGenerator;
            }

            set
            {
                this.randomNumberGenerator = value;
            }
        }

        public Player AddPlayer(long id, bool isLocal)
        {
            if (this.players.ContainsKey(id))
            {
                return this.players[id];
            }

            var player = new Player(id, this.spriteSheet, this.inputManager, this.cardManager);

            this.players.Add(player.Id, player);

            if (isLocal)
            {
                this.localPlayer = player;
            }

            return player;
        }

        public Player AddPlayer(bool isLocal)
        {
            Player player = this.AddPlayer(Interlocked.Increment(ref playerIdCounter), isLocal); 

            return player;
        }

        public bool PayerIsLocal(Player player)
        {
            return this.localPlayer != null && this.localPlayer.Id == player.Id;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (Player player in this.Players)
            {
                _destRect = new Rectangle(IncludeConstant.leftMargin + IncludeConstant.localCardPositions[i], IncludeConstant.playerCardPosition_Y,
                    IncludeConstant.FrameWidth - IncludeConstant.pixel, IncludeConstant.FrameHeight);

                player.Draw(spriteBatch, _destRect);

                i++;
            }
        }

        public Player GetPlayer(long id)
        {
            if (this.players.ContainsKey(id))
            {
                return this.players[id];
            }

            return null;
        }

        public void LoadContent(ContentManager contentManager)
        {
            this.spriteSheet = contentManager.Load<Texture2D>(@"Textures\SpriteSheet");
            this.hearbeatTimer = new GameTimer();
        }

        public bool isPlayerLocal(Player player)
        {
            return this.localPlayer != null && this.localPlayer.Id == player.Id;
        }

        public void RemovePlayer(long id)
        {
            if (this.players.ContainsKey(id))
            {
                this.players.Remove(id);
            }
        }

        public void Update(GameTime gameTime)
        {
            if ((this.localPlayer != null))
            {
                // Any local changes go here
                if (this.inputManager.isLeftButtonClicked())
                {
                    this.localPlayer.Update(gameTime);

                    this.OnPlayerStateChanged(localPlayer);
                }
            }

            foreach (Player player in this.Players)
            {
                //player.Update(gameTime);
            }

            if (this.isHost && this.hearbeatTimer.Stopwatch(1000))
            {
                Console.WriteLine("NETWORK DUMP >>>>");
                foreach (Player player in this.Players)
                {
                    Console.WriteLine("Player Id: {0} Card: {1}", player.Id, player.CardIndex);

                    this.OnPlayerStateChanged(player);
                }
            }

        }

        protected void OnPlayerStateChanged(Player player)
        {
            EventHandler<PlayerStateChangedArgs> playerStateChanged = this.PlayerStateChanged;
            if (playerStateChanged != null)
            {
                playerStateChanged(this, new PlayerStateChangedArgs(player));
            }
        }

    }
}
