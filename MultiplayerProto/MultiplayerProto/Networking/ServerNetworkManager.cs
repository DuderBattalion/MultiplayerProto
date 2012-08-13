using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Networking
{
    using System;

    using Lidgren.Network;

    using MultiplayerProto.Networking.Messages;

    public class ServerNetworkManager: INetworkManager
    {
        private NetServer netServer;

        private bool isDisposed;

        public void Connect()
        {
            var config = new NetPeerConfiguration("CAH")
            {
                Port = Convert.ToInt32("14242")
            };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.netServer = new NetServer(config);
            this.netServer.Start();
        }

        public NetOutgoingMessage CreateMessage()
        {
            return this.netServer.CreateMessage();
        }

        public void Disconnect()
        {
            this.netServer.Shutdown("Bye");
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public NetIncomingMessage ReadMessage()
        {
            return this.netServer.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            this.netServer.Recycle(im);
        }

        public void SendMessage(IGameMessage gameMessage)
        {
            NetOutgoingMessage om = this.netServer.CreateMessage();
            om.Write((byte)gameMessage.MessageType);
            gameMessage.Encode(om);

            this.netServer.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.Disconnect();
                }

                this.isDisposed = true;
            }
        }
    }
}
