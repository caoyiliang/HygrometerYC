using Communication;
using Communication.Bus.PhysicalPort;
using Communication.Exceptions;
using HygrometerYC.Request;
using HygrometerYC.Response;
using LogInterface;
using Parser.Parsers;
using TopPortLib;
using TopPortLib.Interfaces;
using Utils;

namespace HygrometerYC
{
    public class HygrometerYC : IHygrometerYC
    {
        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<HygrometerYC>();
        private readonly ICrowPort _crowPort;
        private bool _isConnect = false;
        public bool IsConnect => _isConnect;

        /// <inheritdoc/>
        public event DisconnectEventHandler? OnDisconnect { add => _crowPort.OnDisconnect += value; remove => _crowPort.OnDisconnect -= value; }
        /// <inheritdoc/>
        public event ConnectEventHandler? OnConnect { add => _crowPort.OnConnect += value; remove => _crowPort.OnConnect -= value; }

        public HygrometerYC(SerialPort serialPort, int defaultTimeout = 5000)
        {
            _crowPort = new CrowPort(new TopPort(serialPort, new TimeParser(200)), defaultTimeout);
            _crowPort.OnSentData += CrowPort_OnSentData;
            _crowPort.OnReceivedData += CrowPort_OnReceivedData;
            _crowPort.OnConnect += CrowPort_OnConnect;
            _crowPort.OnDisconnect += CrowPort_OnDisconnect;
        }

        public HygrometerYC(ICrowPort crowPort)
        {
            _crowPort = crowPort;
            _crowPort.OnConnect += CrowPort_OnConnect;
            _crowPort.OnDisconnect += CrowPort_OnDisconnect;
        }

        private async Task CrowPort_OnDisconnect()
        {
            _isConnect = false;
            await Task.CompletedTask;
        }

        private async Task CrowPort_OnConnect()
        {
            _isConnect = true;
            await Task.CompletedTask;
        }

        private async Task CrowPort_OnReceivedData(byte[] data)
        {
            _logger.Trace($"HygrometerYC Rec:<-- {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        private async Task CrowPort_OnSentData(byte[] data)
        {
            _logger.Trace($"HygrometerYC Send:--> {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task OpenAsync()
        {
            _isConnect = _crowPort.PhysicalPort.IsOpen;
            return _crowPort.OpenAsync();
        }

        /// <inheritdoc/>
        public async Task CloseAsync(bool closePhysicalPort)
        {
            if (closePhysicalPort) await _crowPort.CloseAsync();
        }

        public async Task<Dictionary<string, string>?> Read(string addr)
        {
            if (!_isConnect) throw new NotConnectedException();
            var rs = await _crowPort.RequestAsync<ReadReq, ReadRsp>(new ReadReq(addr));
            var rs1 = await _crowPort.RequestAsync<ReadRangeReq, ReadRangeRsp>(new ReadRangeReq(addr));
            var rs2 = await _crowPort.RequestAsync<ReadStateReq, ReadStateRsp>(new ReadStateReq(addr));

            return new Dictionary<string, string>()
            {
                { "4108", rs.氧气.ToString() },
                { "4110", rs.湿度.ToString() },
                { "4108-state", rs2.状态 switch
                {
                    0 => "N",
                    1 => "M",
                    2 => "D",
                    3 => "C",
                    4 => "C",
                    5 => "N",
                    6 => "N",
                    7 => "M",
                    8 => "N",
                    _ => "N"
                }},
                { "4110-state", rs2.状态 switch
                {
                    0 => "N",
                    1 => "M",
                    2 => "D",
                    3 => "N",
                    4 => "N",
                    5 => "C",
                    6 => "C",
                    7 => "M",
                    8 => "N",
                    _ => "N"
                }},
                { "S01-FullRange", rs1.氧气量程.ToString() },
                { "S05-FullRange", rs1.湿度量程.ToString() }
            };
        }
    }
}
