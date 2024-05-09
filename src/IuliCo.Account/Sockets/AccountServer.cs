using System.Net;
using System.Net.Sockets;
using IuliCo.Core;
using IuliCo.Core.Enums;

namespace IuliCo.Account.Sockets
{
    class AccountServer
    {
        private int _port;
        private Socket _listener;

        public AccountServer(int port)
        {
            _port = port;
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task StartAsync()
        {
            // Logging:
            await AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Starting server on port {_port}");
            _listener.Bind(new IPEndPoint(IPAddress.Any, _port));
            _listener.Listen(10);

            while (true)
            {
                var clientSocket = await _listener.AcceptAsync();
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Client connected: {clientSocket.RemoteEndPoint}");
                _ = HandleClientAsync(clientSocket);
            }
        }

        private async Task HandleClientAsync(Socket client)
        {
            try
            {
                using (var stream = new NetworkStream(client))
                {
                    while (client.Connected)
                    {
                        byte[] buffer = new byte[2048]; // Adjust buffer size as needed
                        int bytesReceived = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesReceived == 0)
                        {
                            await AsyncLogger.Instance.LogAsync(LogLevel.Info, $"Client disconnected: {client.RemoteEndPoint}");
                            break;
                        }

                        // Process the received data (decrypt, process, potentially encrypt and send a response)
                        byte[] processedData = await ProcessDataAsync(buffer, bytesReceived);

                        if (processedData != null)
                        {
                            await stream.WriteAsync(processedData, 0, processedData.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // _logger?.LogError(ex, "Error handling client");
                await AsyncLogger.Instance.LogAsync(LogLevel.Error, $"Error handling client: {ex.Message}");
            }
        }

        // Implement the ProcessDataAsync method for handling encryption/decryption and message processing
        private async Task<byte[]> ProcessDataAsync(byte[] receivedData, int bytesReceived)
        {
            // 1. Perform decryption (using your adapted `AuthCryptography` class)
            // 2. Deserialize and process the incoming message
            // 3. Prepare a response message (if necessary)
            // 4. Serialize the response
            // 5. Encrypt the response
            // 6. Return the encrypted response byte array
            return new byte[0];
        }
    }
}
