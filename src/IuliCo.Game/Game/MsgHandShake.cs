namespace IuliCo.Game.Game
{
    public static class MsgHandShake
    {
        public static ServerSockets.Packet Handshake(this ServerSockets.Packet msg, byte[] p, byte[] g, byte[] publicKey)
        {
            var size = 75 + p.Length + g.Length + publicKey.Length;
            msg.Seek(11);                   // padding
            msg.Write(size - 11);           // total size minus padding
            msg.Write(10);                  // junk length
            msg.SeekForward(10);            // skip junk
            msg.Write(8);                   // client ivec
            msg.ZeroFill(8);                // zero fill for client ivec
            msg.Write(8);                   // server ivec
            msg.ZeroFill(8);                // zero fill for server ivec
            msg.Write(p.Length);            // length of p
            foreach (byte b in p)           // iterate and write each byte of p
                msg.Write(b);
            msg.Write(g.Length);            // length of g
            foreach (byte b in g)           // iterate and write each byte of g
                msg.Write(b);
            msg.Write(publicKey.Length);    // length of publicKey
            foreach (byte b in publicKey)   // iterate and write each byte of publicKey
                msg.Write(b);
            msg.ZeroFill(2);                // zero fill 2 spaces
            msg.WriteSeal();
            msg.Size = msg.Position;        // update packet size to current position
            return msg;
        }
    }
}
