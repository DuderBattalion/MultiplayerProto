using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MultiplayerProto.Entities;

namespace MultiplayerProto.EventArgs
{
    using System;

    public class PlayerStateChangedArgs: EventArgs
    {
        public Player Player { get; private set; }

        public PlayerStateChangedArgs(Player player)
        {
            this.Player = player;
        }
    }
}
