

using IuliCo.Account.Account;
using IuliCo.Account.Account.Client;
using IuliCo.Account.Account.Cryptography;
using IuliCo.Account.Sockets;
using IuliCo.Core;
using IuliCo.Core.Enums;

namespace IuliCo.Account
{
    class Program
    {
        public static System.FastRandom Random = new();
        public static AccountServer AuthServer = new();
        public static World World = new();
        public static ushort Port = 9958;// 9958 is the default port
        public static Time32 Login = Time32.Now;
        private static object SyncLogin = new();
        public static string ServerLoaded = "";
        public static int ReceivedLogins = 0, AcceptedLogins = 0, RejectedLogins = 0;
        // public static AccountServerConfig ASConfig { get; set; }
        // private static void WorkConsole()
        // {
        //     while (true)
        //     {
        //         try
        //         {
        //             CommandsAI(Console.ReadLine());
        //         }
        //         catch (Exception e)
        //         {
        //             Console.WriteLine(e);
        //         }
        //     }
        // }
        // #region LogWritter
        public static string StartupPath = "";
        private static Counter ExceptionsCounter = new Counter(1);
        public static void WriteException(Exception e)
        {
            try
            {
                // Console.WriteLine(e.ToString());
                AsyncLogger.Instance.LogAsync(LogLevel.Error, e.ToString()).ConfigureAwait(false);
                // SaveException(e);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());
                AsyncLogger.Instance.LogAsync(LogLevel.Error, ex.ToString()).ConfigureAwait(false);
            }
        }
        //         public static void SaveException(Exception e)
        //         {
        //             const string UnhandledExceptionsPath = "Exceptions";

        //             var dt = DateTime.Now;
        //             string date = dt.Month + "-" + dt.Day + Path.DirectorySeparatorChar;

        //             if (!Directory.Exists(Path.Combine(Program.StartupPath, UnhandledExceptionsPath)))
        //                 Directory.CreateDirectory(Path.Combine(Program.StartupPath, UnhandledExceptionsPath));
        //             if (!Directory.Exists(Path.Combine(Program.StartupPath, UnhandledExceptionsPath, date)))
        //                 Directory.CreateDirectory(Path.Combine(Program.StartupPath, UnhandledExceptionsPath, date));
        //             if (!Directory.Exists(Path.Combine(Program.StartupPath, UnhandledExceptionsPath, date, e.TargetSite.Name)))
        //                 Directory.CreateDirectory(Path.Combine(Program.StartupPath, UnhandledExceptionsPath, date, e.TargetSite.Name));

        //             string fullPath = Path.Combine(Program.StartupPath, UnhandledExceptionsPath, date, e.TargetSite.Name);

        //             string date2 = dt.DayOfYear + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + "E" + ExceptionsCounter.Next;
        //             List<string> Lines = new List<string>();

        //             Lines.Add("----Exception message----");
        //             Lines.Add(e.Message);
        //             Lines.Add("----End of exception message----\r\n");

        //             Lines.Add("----Stack trace----");
        //             Lines.Add(e.StackTrace);
        //             Lines.Add("----End of stack trace----\r\n");

        //             File.WriteAllLines(fullPath + date2 + ".txt", Lines.ToArray());
        //             Console.WriteLine(e.ToString());
        //         }
        // #endregion
        private static void Main(string[] args)
        {
            // Program.StartupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            try
            {
                // if (args.Length > 0)
                // {
                //     // RestApiHelper.ApiPort = ushort.Parse(args[0]);
                //     // RestApiHelper.Init();
                //     Port++;
                //     // Console.WriteLine($"[API URI: {RestApiHelper.ApiRequestBaseURI}]");

                // }
                // Console.WriteLine("Starting AccountServer....");
                AsyncLogger.Instance.LogAsync(LogLevel.Info, "Starting AccountServer....").ConfigureAwait(false);
                Thread.Sleep(1000);
                AsyncLogger.Instance.LogAsync(LogLevel.Info, "Checking db connection...").ConfigureAwait(false);
                // Console.WriteLine("Checking db connection...");
                // bool canConnect = false;
                // try
                // {
                //     canConnect = Utils.CanConnect();
                // }
                // catch (Exception)
                // {
                //     Console.WriteLine("API Down?");
                // }
                // if (!canConnect)
                // {
                //     Console.WriteLine("Cannot connect to the database auth with your configuration. Press Any key for close.");
                //     Console.Read();
                //     Environment.Exit(0);
                //     return;
                // }
                // ASConfig = RestApiHelper.GetRequest<AccountServerConfig>("GetASConfig");
                // List<Server> AvailableServers = RestApiHelper.GetRequest<List<Server>>("Servers");
                // Console.WriteLine($"Loading server: {ASConfig.ServerName}.");
                // Server defaultTrinityServer = AvailableServers.Find(x => x.Name == ASConfig.ServerName);
                // Server serverLoadedObj = defaultTrinityServer;
                // if (defaultTrinityServer != null)
                // {
                //     ServerLoaded = defaultTrinityServer.Name;
                // }
                // else
                // {
                //     Console.WriteLine($"Cannot find the Server {ASConfig.ServerName} in Database. Press Any key for close.");
                //     Console.Read();
                //     Environment.Exit(0);
                // }
                // Console.Title = $"[{ServerLoaded}] - AccountServer - Received Logins: [{ReceivedLogins}] - Accepted: [{AcceptedLogins}] - Rejected: [{RejectedLogins}]";
                // Console.ForegroundColor = ConsoleColor.White;
                World = new World();
                World.Init();
                SyncLogin = new object();
                // Server.Load(serverLoadedObj);
                Console.WriteLine("Starting the server...");
                AuthCryptography.PrepareAuthCryptography();
                AuthServer = new AccountServer();
                AuthServer.OnClientConnect += AuthServer_OnClientConnect;
                AuthServer.OnClientReceive += AuthServer_OnClientReceive;
                AuthServer.OnClientDisconnect += AuthServer_OnClientDisconnect;
                AuthServer.Enable(Port, "0.0.0.0");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Connection Port " + Port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The server is ready for incoming connections!\n");
                Console.ForegroundColor = ConsoleColor.White;
                // WorkConsole();
                // CommandsAI(Console.ReadLine());
            }
            catch (Exception e) { WriteException(e); }
        }
        // public static void CommandsAI(string command)
        // {
        //     if (command == null) return;
        //     string[] data = command.Split(' ');
        //     switch (data[0])
        //     {
        //         case "@memoryusage":
        //             {
        //                 var proc = System.Diagnostics.Process.GetCurrentProcess();
        //                 Console.WriteLine("Thread count: " + proc.Threads.Count);
        //                 Console.WriteLine("Memory set(MB): " + ((double)((double)proc.WorkingSet64 / 1024)) / 1024);
        //                 proc.Close();
        //                 break;
        //             }
        //         case "@clear":
        //             {
        //                 Console.Clear();
        //                 break;
        //             }
        //         case "@exit":
        //             {
        //                 AuthServer.Disable();
        //                 Environment.Exit(0);
        //                 break;
        //             }
        //     }
        // }

        private static void AuthServer_OnClientReceive(byte[] buffer, int length, ClientWrapper arg3)
        {

            var player = arg3.Connector as AuthClient;
            player!.Cryptographer.Decrypt(buffer, length);
            player!.Queue.Enqueue(buffer, length);
            while (player!.Queue.CanDequeue())
            {
                byte[] packet = player.Queue.Dequeue();
                ushort len = BitConverter.ToUInt16(packet, 0);
                ushort id = BitConverter.ToUInt16(packet, 2);
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
                //         if (Core.Models.Server.Servers.TryGetValue(ServerSelected, out ServerInfo Server))
                //         {
                //             if (!player.Account.Exists())
                //             {
                //                 Fw.Type = Forward.ForwardType.WrongAccount;
                //             }

                //             if (player.Account.Password == player.Info.Password && player.Account.Exists())
                //             {
                //                 Fw.Type = Forward.ForwardType.Ready;

                //                 if (player.Account.EntityID == 0)
                //                 {
                //                     Fw.Type = Forward.ForwardType.InvalidAuthenticationProtocol;
                //                 }
                //             }
                //             if (Fw.Type != Forward.ForwardType.Ready)
                //             {
                //                 Fw.Type = Forward.ForwardType.InvalidInfo;
                //             }
                //             if (player.Account.State == Account.AccountState.Banned)
                //             {
                //                 Fw.Type = Forward.ForwardType.Banned;
                //             }
                //             lock (SyncLogin)
                //             {
                //                 if (Fw.Type == Forward.ForwardType.Ready)
                //                 {
                //                     TransferCipher transferCipher = new(Server.TransferKey, Server.TransferSalt, "127.0.0.1"); // If AddressIP is different of 127.0.0.1 not working good
                //                     uint[] encrypted = transferCipher.Encrypt(new uint[] { player.Account.EntityID, (uint)player.Account.State });
                //                     Program.AcceptedLogins++;
                //                     Fw.Identifier = encrypted[0];
                //                     Fw.State = (uint)encrypted[1];
                //                     Fw.IP = Server.IP;
                //                     Fw.Port = Server.Port;
                //                     Console.ForegroundColor = ConsoleColor.DarkYellow;
                //                     RestApiHelper.PostRequestSuccessful("Account", player.Account);
                //                     Console.WriteLine("{0} has been Login to server {1}! IP:[{2}].", player.Info.Username, player.Info.Server, player.IP);
                //                 }
                //                 player.Send(Fw);
                //             }
                //         }
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

// using IuliCo.Account.Account;
// using IuliCo.Account.Account.Client;
// using IuliCo.Account.Account.Cryptography;
// using IuliCo.Account.Sockets;
// using IuliCo.Core;
// using IuliCo.Core.Enums;
// using IuliCo.Database;
// using Microsoft.EntityFrameworkCore;

// namespace IuliCo.Account
// {

//     class Program
//     {
//         public static World World = new();
//         private const int Port = 9958;
//         private static AccountServer? AuthServer;
//         public static Random Random = new();
//         static async Task Main(string[] args)
//         {
//             // Create an instance of DbContext using the factory
//             var dbContextFactory = new AccountContextFactory();
//             using (var dbContext = dbContextFactory.CreateDbContext(args))
//             {
//                 await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Applying pending migrations to the database.");
//                 // Apply pending migrations to the database
//                 await dbContext.Database.MigrateAsync();
//                 await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Migrations applied.");
//             }
//             AuthCryptography.PrepareAuthCryptography();
//             AuthServer = new AccountServer();
//             AuthServer.OnClientConnect += AuthServer_OnClientConnect;
//             AuthServer.OnClientReceive += AuthServer_OnClientReceive;
//             AuthServer.OnClientDisconnect += AuthServer_OnClientDisconnect;
//             AuthServer.Enable(Port, "0.0.0.0");
//         }
//         private static void AuthServer_OnClientReceive(byte[] buffer, int length, ClientWrapper arg3)
//         {

//             var player = arg3.Connector as AuthClient;
//             if (player == null)
//             {
//                 return;
//             }
//             player.Cryptographer.Decrypt(buffer, length);
//             player.Queue.Enqueue(buffer, length);
//             while (player.Queue.CanDequeue())
//             {
//                 byte[] packet = player.Queue.Dequeue();
//                 ushort len = BitConverter.ToUInt16(packet, 0);
//                 ushort id = BitConverter.ToUInt16(packet, 2);
//                 AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Received packet with ID {id} and length {len}.").ConfigureAwait(false);
//                 //     if (id == 1124 && len == 240)
//                 //     {
//                 //         player.Info = new Authentication { };
//                 //         player.Info.Deserialize(packet);
//                 //         Dictionary<string, string> param = new Dictionary<string, string>();
//                 //         param.Add("Username", player.Info.Username);
//                 //         Account acc = RestApiHelper.GetRequest<Account>("Account", param);
//                 //         player.Account = acc;
//                 //         Forward Fw = new();
//                 //         if (player.Account == null || player.Info.Username.Length <= 0) // Not valid account has find or cannot get the username from client
//                 //         {
//                 //             Fw.Type = Forward.ForwardType.InvalidInfo;
//                 //             player.Send(Fw);
//                 //             return;
//                 //         }
//                 //         player.Account.IP = arg3.IP;
//                 //         string ServerSelected = player.Info.Server;
//                 //         bool anyServerSelected = true;
//                 //         if (anyServerSelected)
//                 //         {
//                 //             ServerSelected = ServerLoaded;
//                 //         }
//                 //         // if (Core.Models.Server.Servers.TryGetValue(ServerSelected, out ServerInfo Server))
//                 //         // {
//                 //         //     // if (!player.Account.Exists())
//                 //         //     // {
//                 //         //     //     Fw.Type = Forward.ForwardType.WrongAccount;
//                 //         //     // }

//                 //         //     // if (player.Account.Password == player.Info.Password && player.Account.Exists())
//                 //         //     // {
//                 //         //     //     Fw.Type = Forward.ForwardType.Ready;

//                 //         //     //     if (player.Account.EntityID == 0)
//                 //         //     //     {
//                 //         //     //         Fw.Type = Forward.ForwardType.InvalidAuthenticationProtocol;
//                 //         //     //     }
//                 //         //     // }
//                 //         //     // if (Fw.Type != Forward.ForwardType.Ready)
//                 //         //     // {
//                 //         //     //     Fw.Type = Forward.ForwardType.InvalidInfo;
//                 //         //     // }
//                 //         //     // if (player.Account.State == Account.AccountState.Banned)
//                 //         //     // {
//                 //         //     //     Fw.Type = Forward.ForwardType.Banned;
//                 //         //     // }
//                 //         //     // lock (SyncLogin)
//                 //         //     // {
//                 //         //     //     if (Fw.Type == Forward.ForwardType.Ready)
//                 //         //     //     {
//                 //         //     //         TransferCipher transferCipher = new(Server.TransferKey, Server.TransferSalt, "127.0.0.1"); // If AddressIP is different of 127.0.0.1 not working good
//                 //         //     //         uint[] encrypted = transferCipher.Encrypt(new uint[] { player.Account.EntityID, (uint)player.Account.State });
//                 //         //     //         Program.AcceptedLogins++;
//                 //         //     //         Fw.Identifier = encrypted[0];
//                 //         //     //         Fw.State = (uint)encrypted[1];
//                 //         //     //         Fw.IP = Server.IP;
//                 //         //     //         Fw.Port = Server.Port;
//                 //         //     //         Console.ForegroundColor = ConsoleColor.DarkYellow;
//                 //         //     //         // RestApiHelper.PostRequestSuccessful("Account", player.Account);
//                 //         //     //         Console.WriteLine("{0} has been Login to server {1}! IP:[{2}].", player.Info.Username, player.Info.Server, player.IP);
//                 //         //     //     }
//                 //         //     //     player.Send(Fw);
//                 //         //     // }
//                 //         // }
//                 //         // else
//                 //         else
//                 //             arg3.Disconnect();
//                 //     }
//             }
//         }
//         private static void AuthServer_OnClientDisconnect(ClientWrapper obj)
//         {
//             obj.Disconnect();
//         }
//         private static void AuthServer_OnClientConnect(ClientWrapper obj)
//         {
//             AuthClient authState;
//             obj.Connector = authState = new AuthClient(obj);
//             authState.Cryptographer = new AuthCryptography();
//             PasswordCryptographySeed pcs = new()
//             {
//                 Seed = Random.Next()
//             };
//             authState.PasswordSeed = pcs.Seed;
//             authState.Send(pcs);
//         }

//     }
// }