
using IuliCo.Account.Account;
using IuliCo.Account.Account.Client;
using IuliCo.Account.Account.Cryptography;
using IuliCo.Account.Sockets;
using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Database;
using Microsoft.EntityFrameworkCore;

namespace IuliCo.Account
{

    class Program
    {
        public static World World = new();
        private const int Port = 9958;
        private static AccountServer? AuthServer;
        public static Random Random = new();
        static async Task Main(string[] args)
        {
            // Create an instance of DbContext using the factory
            var dbContextFactory = new AccountContextFactory();
            using (var dbContext = dbContextFactory.CreateDbContext(args))
            {
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Applying pending migrations to the database.");
                // Apply pending migrations to the database
                await dbContext.Database.MigrateAsync();
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Migrations applied.");
            }
            AuthCryptography.PrepareAuthCryptography();
            AuthServer = new AccountServer();
            AuthServer.OnClientConnect += AuthServer_OnClientConnect;
            AuthServer.OnClientReceive += AuthServer_OnClientReceive;
            AuthServer.OnClientDisconnect += AuthServer_OnClientDisconnect;
            AuthServer.Enable(Port, "0.0.0.0");
        }
        private static void AuthServer_OnClientReceive(byte[] buffer, int length, ClientWrapper arg3)
        {

            var player = arg3.Connector as AuthClient;
            if (player == null)
            {
                return;
            }
            player.Cryptographer.Decrypt(buffer, length);
            player.Queue.Enqueue(buffer, length);
            while (player.Queue.CanDequeue())
            {
                byte[] packet = player.Queue.Dequeue();
                ushort len = BitConverter.ToUInt16(packet, 0);
                ushort id = BitConverter.ToUInt16(packet, 2);
                AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Received packet with ID {id} and length {len}.").ConfigureAwait(false);
                //     if (id == 1124 && len == 240)
                //     {
                //         player.Info = new Authentication { };
                //         player.Info.Deserialize(packet);
                //         Dictionary<string, string> param = new Dictionary<string, string>();
                //         param.Add("Username", player.Info.Username);
                //         Account acc = RestApiHelper.GetRequest<Account>("Account", param);
                //         player.Account = acc;
                //         Forward Fw = new();
                //         if (player.Account == null || player.Info.Username.Length <= 0) // Not valid account has find or cannot get the username from client
                //         {
                //             Fw.Type = Forward.ForwardType.InvalidInfo;
                //             player.Send(Fw);
                //             return;
                //         }
                //         player.Account.IP = arg3.IP;
                //         string ServerSelected = player.Info.Server;
                //         bool anyServerSelected = true;
                //         if (anyServerSelected)
                //         {
                //             ServerSelected = ServerLoaded;
                //         }
                //         // if (Core.Models.Server.Servers.TryGetValue(ServerSelected, out ServerInfo Server))
                //         // {
                //         //     // if (!player.Account.Exists())
                //         //     // {
                //         //     //     Fw.Type = Forward.ForwardType.WrongAccount;
                //         //     // }

                //         //     // if (player.Account.Password == player.Info.Password && player.Account.Exists())
                //         //     // {
                //         //     //     Fw.Type = Forward.ForwardType.Ready;

                //         //     //     if (player.Account.EntityID == 0)
                //         //     //     {
                //         //     //         Fw.Type = Forward.ForwardType.InvalidAuthenticationProtocol;
                //         //     //     }
                //         //     // }
                //         //     // if (Fw.Type != Forward.ForwardType.Ready)
                //         //     // {
                //         //     //     Fw.Type = Forward.ForwardType.InvalidInfo;
                //         //     // }
                //         //     // if (player.Account.State == Account.AccountState.Banned)
                //         //     // {
                //         //     //     Fw.Type = Forward.ForwardType.Banned;
                //         //     // }
                //         //     // lock (SyncLogin)
                //         //     // {
                //         //     //     if (Fw.Type == Forward.ForwardType.Ready)
                //         //     //     {
                //         //     //         TransferCipher transferCipher = new(Server.TransferKey, Server.TransferSalt, "127.0.0.1"); // If AddressIP is different of 127.0.0.1 not working good
                //         //     //         uint[] encrypted = transferCipher.Encrypt(new uint[] { player.Account.EntityID, (uint)player.Account.State });
                //         //     //         Program.AcceptedLogins++;
                //         //     //         Fw.Identifier = encrypted[0];
                //         //     //         Fw.State = (uint)encrypted[1];
                //         //     //         Fw.IP = Server.IP;
                //         //     //         Fw.Port = Server.Port;
                //         //     //         Console.ForegroundColor = ConsoleColor.DarkYellow;
                //         //     //         // RestApiHelper.PostRequestSuccessful("Account", player.Account);
                //         //     //         Console.WriteLine("{0} has been Login to server {1}! IP:[{2}].", player.Info.Username, player.Info.Server, player.IP);
                //         //     //     }
                //         //     //     player.Send(Fw);
                //         //     // }
                //         // }
                //         // else
                //         else
                //             arg3.Disconnect();
                //     }
            }
        }
        private static void AuthServer_OnClientDisconnect(ClientWrapper obj)
        {
            obj.Disconnect();
        }
        private static void AuthServer_OnClientConnect(ClientWrapper obj)
        {
            AuthClient authState;
            obj.Connector = authState = new AuthClient(obj);
            authState.Cryptographer = new AuthCryptography();
            PasswordCryptographySeed pcs = new()
            {
                Seed = Random.Next()
            };
            authState.PasswordSeed = pcs.Seed;
            authState.Send(pcs);
        }

    }
}