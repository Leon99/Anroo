using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Anroo.Common.Network;

namespace Anroo.OrviboS20
{
    public sealed class OrviboS20Controller : IDisposable
    {
        private readonly byte[] _thingMac;
        private const int CommandDelay = 500;
        private readonly TimeSpan _subscriptionRenewInterval = TimeSpan.FromMinutes(5);
        private readonly byte[] _subscribeMessage;
        private readonly byte[] _powerOnMessage;
        private readonly byte[] _powerOffMessage;

        private DateTimeOffset _lastSubscriptionTime;

        private readonly UdpTransceiver _transceiver;

        public OrviboS20Controller(IPAddress thingIP, byte[] thingMac)
        {
            _thingMac = thingMac;
            var messageFabric = new MessageFactory(thingMac);
            _powerOnMessage = messageFabric.CreatePowerOnMessage();
            _powerOffMessage = messageFabric.CreatePowerOffMessage();
            _subscribeMessage = messageFabric.CreateSubscribeMessage();

            _transceiver = new UdpTransceiver(
                new IPEndPoint(thingIP, OrviboS20Ports.ThingPort), 
                new IPEndPoint(IPAddress.Any, OrviboS20Ports.ThingPort)) {DelayAfterSend = CommandDelay};
        }

        public async Task<OrviboS20PowerStateResponse> PowerOnAsync()
        {
            return await ExecuteCommandAsync(_powerOnMessage);
        }

        public async Task<OrviboS20PowerStateResponse> PowerOffAsync()
        {
            return await ExecuteCommandAsync(_powerOffMessage);
        }

        private async Task<OrviboS20PowerStateResponse> ExecuteCommandAsync(byte[] message)
        {
            EnsureOperationAllowed();
            return await SendReceiveAsync<OrviboS20PowerStateResponse>(message);
        }

        private async Task<T> SendReceiveAsync<T>(byte[] message) where T : OrviboS20CommandResponse
        {
            OrviboS20CommandResponse response = null;
            int retryNumber = 0;
            while (response == null && retryNumber < 3)
            {
                await _transceiver.SendDataAsync(message);
                await _transceiver.ReceiveUntilAsync(result => OrviboS20CommandResponseFactory.TryParse(result, out response)
                                                              && response is T
                                                              && response.NetworkAddress.MacAddress.SequenceEqual(_thingMac));
                retryNumber++;
            }
            return (T) response;
        }

        private void EnsureOperationAllowed()
        {
            var intervalAfterLastSubscription = DateTimeOffset.Now - _lastSubscriptionTime;
            if (intervalAfterLastSubscription > _subscriptionRenewInterval)
            {
                SendReceiveAsync<OrviboS20SubscribeResponse>(_subscribeMessage).Wait();
                _lastSubscriptionTime = DateTimeOffset.Now;
            }
        }

        public void Dispose()
        {
            _transceiver.Dispose();
        }
    }
}