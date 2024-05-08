using System.Net;
using System.Net.Sockets;
using IuliCo.Core;
using IuliCo.Core.Enums;

namespace IuliCo.Account.Sockets
{
    class AccountServer
    {
        private int port;
        private Socket listener;

        // we will import our Logger class here

        public AccountServer(int port)
        {
            this.port = port;
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task StartAsync()
        {
            await AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Starting Account Server on port {port}");

            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(10); // Start listening for incoming connections

            while (true)
            {
                var clientSocket = await listener.AcceptAsync(); // Asynchronous accept
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Client connected: {clientSocket.RemoteEndPoint}");
                _ = HandleClientAsync(clientSocket); // Handle client asynchronously
            }
        }

        private async Task HandleClientAsync(Socket client)
        {
            try
            {
                // Client interaction (asynchronous handling of receiving/sending data)
            }
            catch (SocketException ex)
            {
                // Log socket-specific errors
                await AsyncLogger.Instance.LogAsync(LogLevel.Error, ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

    }
}
