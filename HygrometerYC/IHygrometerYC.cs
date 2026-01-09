using ProtocolInterface;

namespace HygrometerYC;

public interface IHygrometerYC : IProtocol
{
    Task<Dictionary<string, string>?> Read(string addr);
}
