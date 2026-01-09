using TopPortLib.Interfaces;
using Utils;

namespace HygrometerYC.Request
{
    internal class ReadStateReq : IByteStream
    {
        private string _addr;

        public ReadStateReq(string addr)
        {
            _addr = addr;
        }

        public byte[] ToBytes()
        {
            byte[] cmd = [Convert.ToByte(_addr), 0x03, 0x11, 0x03, 0x00, 0x01];
            return CRC.Crc16(cmd);
        }
    }
}
