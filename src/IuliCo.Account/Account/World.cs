using IuliCo.Account.Account.Client;
using System.Threading.Generic;

namespace IuliCo.Account.Account
{
    public unsafe class World
    {
        public static StaticPool ReceivePool = new StaticPool(32).Run();
        public static StaticPool SendPool = new StaticPool(32).Run();
        public TimerRule<ClientWrapper> ConnectionReceive, ConnectionReview, ConnectionSend;

        public void Init()
        {
            ConnectionReview = new TimerRule<ClientWrapper>(_ConnectionReview, 1000, ThreadPriority.Lowest);
            ConnectionReceive = new TimerRule<ClientWrapper>(_ConnectionReceive, 1, ThreadPriority.Highest);
            ConnectionSend = new TimerRule<ClientWrapper>(_ConnectionSend, 1, ThreadPriority.Highest);
        }
        public World()
        {
            ReceivePool = new StaticPool(32).Run();
            SendPool = new StaticPool(32).Run();
            ConnectionReceive = new TimerRule<ClientWrapper>(_ConnectionReceive, 1, ThreadPriority.Highest);
            ConnectionReview = new TimerRule<ClientWrapper>(_ConnectionReview, 1000, ThreadPriority.Lowest);
            ConnectionSend = new TimerRule<ClientWrapper>(_ConnectionSend, 1, ThreadPriority.Highest);
        }

        private void _ConnectionReview(ClientWrapper wrapper, int time)
        {
            ClientWrapper.TryReview(wrapper);
        }
        private void _ConnectionReceive(ClientWrapper wrapper, int time)
        {
            ClientWrapper.TryReceive(wrapper);
            // Console.Title = $"[{Program.ServerLoaded}] - AccountServer - Received Logins: [{Program.ReceivedLogins}] - Accepted: [{Program.AcceptedLogins}] - Rejected: [{Program.RejectedLogins}]";
        }
        private void _ConnectionSend(ClientWrapper wrapper, int time)
        {
            ClientWrapper.TrySend(wrapper);
        }

        public static IDisposable Subscribe<T>(TimerRule<T> rule, T param, StaticPool pool)
        {
            return pool.Subscribe<T>(rule, param);
        }
        public static IDisposable Subscribe<T>(TimerRule<T> rule, T param, StandalonePool pool)
        {
            return pool.Subscribe<T>(rule, param);
        }

    }
}