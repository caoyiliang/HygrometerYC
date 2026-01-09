using TopPortLib.Interfaces;
using Utils;

namespace HygrometerYC.Request
{
    internal class ReadReq : IByteStream
    {
        private string _addr;

        public ReadReq(string addr)
        {
            _addr = addr;
        }

        public byte[] ToBytes()
        {
            byte[] cmd = [Convert.ToByte(_addr), 0x03, 0x10, 0x0C, 0x00, 0x04];
            return CRC.Crc16(cmd);
        }
    }
}
