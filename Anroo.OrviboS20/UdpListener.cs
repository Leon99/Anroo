using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Anroo.OrviboS20
{
    internal class UdpListener : IDisposable
    {
        public event EventHandler<UdpReceiveResult> DatagramReceived;

        private readonly UdpClient _client;
        private readonly ConcurrentQueue<UdpReceiveResult> _resultsQueue = new ConcurrentQueue<UdpReceiveResult>();
        private readonly AutoResetEvent _resultsQueuedEvent = new AutoResetEvent(false);
        private Func<UdpReceiveResult, bool> _criteriaCheckFunc;
        private bool _endRequested;

        public UdpListener(IPEndPoint localEP)
        {
            _client = new UdpClient(localEP);
        }
        public UdpListener(IPEndPoint localEP, IPEndPoint remoteEp)
        {
            _client = new UdpClient(localEP);
            _client.Connect(remoteEp);
        }

        public async Task StartListeningAsync(TimeSpan timeout = default(TimeSpan), Func<UdpReceiveResult, bool> criteriaCheckFunc = null)
        {
            if (_client.Client.Connected)
            {
                throw new InvalidOperationException("Listening has already been started.");
            }
            _criteriaCheckFunc = criteriaCheckFunc;
            Timer timeoutTimer;
            Task.Run(() => ProcessReceivedResults());
            if (timeout > TimeSpan.Zero)
            {
                timeoutTimer = new Timer(state =>
                {
                    Dispose();
                }, null, timeout, TimeSpan.Zero);
            }
            while (!_endRequested)
            {
                var result = await _client.ReceiveAsync();
                RegisterDatagramForProcessing(result);
            }
        }

        protected virtual void RegisterDatagramForProcessing(UdpReceiveResult e)
        {
            //Trace.TraceInformation($"Datagram of {e.Buffer.Length} bytes received from {e.RemoteEndPoint.Address}.");
            _resultsQueue.Enqueue(e);
            _resultsQueuedEvent.Set();
        }

        private void ProcessReceivedResults()
        {
            Trace.TraceInformation("Waiting for received datagrams to process...");
            while (!_endRequested)
            {
                _resultsQueuedEvent.WaitOne();
                UdpReceiveResult result;
                while (!_endRequested && _resultsQueue.TryDequeue(out result))
                {
                    if (_criteriaCheckFunc != null)
                    {
                        if (_criteriaCheckFunc(result))
                        {
                            Dispose();
                            OnDatagramReceived(result);
                        }
                    }
                    else
                    {
                        OnDatagramReceived(result);
                    }
                }
            }
        }

        protected virtual void OnDatagramReceived(UdpReceiveResult result)
        {
            Trace.TraceInformation($"Notifying about datagram of {result.Buffer.Length} bytes from {result.RemoteEndPoint.Address}...");
            DatagramReceived?.Invoke(this, result);
        }

        public void Dispose()
        {
            _endRequested = true;
            _client.Client.Dispose();
            _resultsQueuedEvent.Set();
        }
    }
}