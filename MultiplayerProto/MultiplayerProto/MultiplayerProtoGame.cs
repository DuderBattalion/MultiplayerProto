using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MultiplayerProto
{
    using Lidgren.Network;
    
    using MultiplayerProto.Networking;
    using MultiplayerProto.RandomNumbers;
    using MultiplayerProto.Managers;
    using MultiplayerProto.Networking.Messages;
    using MultiplayerProto.Entities;
    using MultiplayerProto.Constants;


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MultiplayerProtoGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly INetworkManager networkManager;

        private InputManager inputManager;

        private PlayerManager playerManager;
        private CardManager cardManager;

        private Texture2D spritesheet;

        SpriteFont font;

        public MultiplayerProtoGame(INetworkManager networkManager)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = IncludeConstant.resolutionWidth;
            graphics.PreferredBackBufferHeight = IncludeConstant.resolutionHeigth;

            this.networkManager = networkManager;
        }

        private bool isHost
        {
            get
            {
                return this.networkManager is ServerNetworkManager;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.networkManager.Connect();

            var randomNumberGenerator = new MersenneTwister();
            this.inputManager = new InputManager(this);

            this.cardManager = new CardManager(randomNumberGenerator);

            this.playerManager = new PlayerManager(randomNumberGenerator, this.inputManager, this.cardManager, this.isHost);
            this.playerManager.PlayerStateChanged +=
                (sender, e) => this.networkManager.SendMessage(new UpdatePlayerStateMessage(e.Player));

            // Commenting out .. cards being handled by Card manager above
            //this.cardList = new GameObject[IncludeConstant.NumberOfCards];
            //for (int i = 0; i < IncludeConstant.NumberOfCards; i++)
            //{
            //    // Inits id and Source Rectangle(from spritesheet) for cards
            //    //this.cardList[i] = new GameObject(i, IncludeConstant.FrameWidth * i , 0, IncludeConstant.FrameWidth, IncludeConstant.FrameHeight);
            //    this.cardList[i] = new GameObject(i, i * (IncludeConstant.FrameWidth + IncludeConstant.pixel), 0, IncludeConstant.FrameWidth, IncludeConstant.FrameHeight);                
                
            //    Console.WriteLine("Card: {0} Rectangle coords: {1} {2} {3} {4}", this.cardList[i].Id, this.cardList[i].SourceRect.X, this.cardList[i].SourceRect.Y, this.cardList[i].SourceRect.Width, this.cardList[i].SourceRect.Height);  
            //}

            this.Components.Add(this.inputManager);

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.playerManager.LoadContent(this.Content);
            this.cardManager.LoadContent(this.Content);

            spritesheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");

            if (this.isHost)
            {
                this.playerManager.AddPlayer(true);
            }

            font = Content.Load<SpriteFont>("Courier New");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this.networkManager.Disconnect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            if (this.inputManager.IsKeyPressed(Keys.Escape))
            {
                this.Exit();
            }

            // TODO: Add your update logic here
            this.ProcessNetworkMessages();

            this.playerManager.Update(gameTime);

            base.Update(gameTime);
        }

        private void HandleUpdatePlayerStateMessage(NetIncomingMessage im)
        {
            var message = new UpdatePlayerStateMessage(im);

            Player player = this.playerManager.GetPlayer(message.Id)
                            ??
                            this.playerManager.AddPlayer(message.Id, false);

            player.CardIndex = message.CardIndex;
        }

        private void ProcessNetworkMessages()
        {
            NetIncomingMessage im;

            while ((im = this.networkManager.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(im.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)im.ReadByte())
                        {
                            case NetConnectionStatus.Connected:
                                if (!this.isHost)
                                {
                                    var message = new UpdatePlayerStateMessage(im.SenderConnection.RemoteHailMessage);
                                    this.playerManager.AddPlayer(
                                        message.Id, true);
                                    Console.WriteLine("Connected to {0}", im.SenderEndpoint);
                                }
                                else
                                {
                                    Console.WriteLine("{0} Connected", im.SenderEndpoint);
                                }

                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine(this.isHost ? "{0} Disconnected" : "Disconnected from {0}", im.SenderEndpoint);
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = this.networkManager.CreateMessage();
                                new UpdatePlayerStateMessage(this.playerManager.AddPlayer(false)).Encode(hailMessage);
                                im.SenderConnection.Approve(hailMessage);
                                break;

                        }

                        break;

                    case NetIncomingMessageType.Data:
                        var gameMessageType = (GameMessageTypes)im.ReadByte();
                        switch (gameMessageType)
                        {
                            case GameMessageTypes.UpdatePlayerState:
                                this.HandleUpdatePlayerStateMessage(im);
                                break;
                        }

                        break;
                }

                this.networkManager.Recycle(im);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            this.playerManager.Draw(this.spriteBatch);

            //spriteBatch.Draw(spritesheet, new Rectangle(0, 0, 32 * 40, 32 * 50), Color.White);

            this.cardManager.Draw(spriteBatch);

            //// Draw local cards
            //for (int i = 1; i < IncludeConstant.NumberOfCards; i++) // Skipping Card = 0 (Blank card)
            //{
            //    _destRect = new Rectangle(IncludeConstant.leftMargin + (i * IncludeConstant.FrameWidth), IncludeConstant.localCardPosition_Y,
            //        IncludeConstant.FrameWidth - IncludeConstant.pixel, IncludeConstant.FrameHeight);

            //    cardList[i].Draw(spriteBatch, spritesheet, _destRect);
            //}

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
