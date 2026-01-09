using Utils;

namespace HygrometerYC.Response
{
    internal class ReadStateRsp
    {
        internal ushort 状态;
        public ReadStateRsp(byte[] reqBytes, byte[] rspBytes)
        {
            if (rspBytes.Length < 3)
            {
                throw new Exception("长度不够");
            }
            var crc = CRC.Crc16(rspBytes, rspBytes.Length - 2);
            if (!(crc[0] == rspBytes[^2] && crc[1] == rspBytes[^1]))
            {
                throw new Exception("CRC校验失败");
            }
            状态=StringByteUtils.ToUInt16(rspBytes, 3, true);
        }
    }
}
