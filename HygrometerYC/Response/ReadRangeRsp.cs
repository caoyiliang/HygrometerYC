using Utils;

namespace HygrometerYC.Response
{
    internal class ReadRangeRsp
    {
        internal ushort 氧气量程;
        internal ushort 湿度量程;
        public ReadRangeRsp(byte[] reqBytes, byte[] rspBytes)
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
            氧气量程=StringByteUtils.ToUInt16(rspBytes, 3, true);
            湿度量程=StringByteUtils.ToUInt16(rspBytes, 5, true);
        }
    }
}
