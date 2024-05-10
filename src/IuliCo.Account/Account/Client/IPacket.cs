namespace IuliCo.Account.Account.Client
{
    public unsafe interface IPacket
    {
        byte[] ToArray();
        void Deserialize(byte[] buffer);
    }
}