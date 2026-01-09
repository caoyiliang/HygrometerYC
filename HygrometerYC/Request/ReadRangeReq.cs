using TopPortLib.Interfaces;
using Utils;

namespace HygrometerYC.Request
{
    internal class ReadRangeReq : IByteStream
    {
        private string _addr;

        public ReadRangeReq(string addr)
        {
            _addr = addr;
        }

        public byte[] ToBytes()
        {
            byte[] cmd = [Convert.ToByte(_addr), 0x03, 0x01, 0x00, 0x00, 0x02];
            return CRC.Crc16(cmd);
        }
    }
}
