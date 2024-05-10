

using IuliCo.Account.Account.Cryptography;

namespace IuliCo.Account.Account.Client
{
    public unsafe class AuthClient(
        ClientWrapper socket
        )
    {
        private ClientWrapper _socket = socket;
        public Authentication Info = new Authentication();
        public Database.Entities.Account.Account Account = new Database.Entities.Account.Account();
        public AuthCryptography Cryptographer = new AuthCryptography();
        public int PasswordSeed;
        public ConcurrentPacketQueue Queue = new ConcurrentPacketQueue(0);

        public void Send(byte[] buffer)
        {
            byte[] _buffer = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, _buffer, 0, buffer.Length);
            Cryptographer.Encrypt(_buffer);
            _socket.Send(_buffer);
        }
        public string IP
        {
            get { return _socket.IP; }

        }
        public void Disconnect()
        {
            _socket.Disconnect();
        }
        public void Send(IPacket buffer)
        {
            Send(buffer.ToArray());
        }
    }
}