using Utils;

namespace HygrometerYC.Response
{
    internal class ReadRsp
    {
        internal float 氧气;
        internal float 湿度;
        public ReadRsp(byte[] reqBytes, byte[] rspBytes)
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
            byte[] dcba = [rspBytes[4], rspBytes[3], rspBytes[6], rspBytes[5]];
            氧气 = BitConverter.ToSingle(dcba, 0);
            byte[] dcbb = [rspBytes[8], rspBytes[7], rspBytes[10], rspBytes[9]];
            湿度 = BitConverter.ToSingle(dcbb, 0);
        }
    }
}
