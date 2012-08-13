using System;

namespace Client
{
    using System.Threading;

    using MultiplayerProto;
    using MultiplayerProto.Networking;

#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new MultiplayerProtoGame(new ClientNetworkManager()))
            {
                game.Run();
            }
        }
    }
#endif
}

