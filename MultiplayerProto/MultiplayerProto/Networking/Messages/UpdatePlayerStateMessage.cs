using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerProto.Networking.Messages
{
    using Lidgren.Network;
    using Lidgren.Network.Xna;

    using Microsoft.Xna.Framework;

    using MultiplayerProto.Entities;

    public class UpdatePlayerStateMessage: IGameMessage
    {
        public long Id { get; set; }

        public double MessageTime { get; set; }

        public int CardIndex { get; set; }

        public GameMessageTypes MessageType
        {
            get
            {
                return GameMessageTypes.UpdatePlayerState;
            }
        }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public UpdatePlayerStateMessage()
        {
        }

        public UpdatePlayerStateMessage(Player player)
        {
            this.Id = player.Id;
            this.CardIndex = player.CardIndex;
            this.MessageTime = NetTime.Now;
        }

        public void Decode(NetIncomingMessage im)
        {
            this.Id = im.ReadInt64();
            this.CardIndex = im.ReadInt16();
            this.MessageTime = im.ReadDouble();            
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Id);
            om.Write(this.CardIndex);
            om.Write(this.MessageTime);
        }
    }
}
