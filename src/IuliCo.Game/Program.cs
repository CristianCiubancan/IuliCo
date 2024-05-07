
using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Game.Sockets;

namespace IuliCo.Game
{

    class Program
    {
        static async Task Main(string[] args)
        {
            GameServer server = new GameServer(9958);

            await server.StartAsync();
        }
    }
}


// Logger logger = new Logger();
// await logger.LogAsync(LogLevel.Info, "Starting game server");
// var server = new GameServer(9958);
// await server.StartAsync();
