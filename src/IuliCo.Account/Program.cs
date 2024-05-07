
using IuliCo.Account.Sockets;

namespace IuliCo.Account
{

    class Program
    {
        static async Task Main(string[] args)
        {
            AccountServer server = new AccountServer(9958);

            await server.StartAsync();
        }
    }
}