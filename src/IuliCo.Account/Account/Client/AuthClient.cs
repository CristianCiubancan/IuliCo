
namespace IuliCo.Account.Account.Cryptography
{
    public unsafe class AuthClient
    {
        // private ClientWrapper _socket;
        // public Network.AuthPackets.Authentication Info;
        // public Account Account;
        public AuthCryptography Cryptographer;
        // public int PasswordSeed;
        // public ConcurrentPacketQueue Queue;
        public AuthClient()
        {
            Cryptographer = new AuthCryptography();
            // Queue = new ConcurrentPacketQueue(0);
            // _socket = socket;
        }
        public void Send(byte[] buffer)
        {
            byte[] _buffer = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, _buffer, 0, buffer.Length);
            Cryptographer.Encrypt(_buffer);
            // _socket.Send(_buffer);
        }
        // public string IP
        // {
        //     get { return _socket.IP; }

        // }
        public void Disconnect()
        {
            // _socket.Disconnect();
        }
        // public void Send(Interfaces.IPacket buffer)
        // {
        //     Send(buffer.ToArray());
        // }
    }
}