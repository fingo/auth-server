using System.Collections.Generic;

namespace Fingo.Auth.AuthServer.Services.Interfaces
{
    public interface IJwtLibraryWrapperService
    {
        string Encode(Dictionary<string , object> payload , string secretKey);
        DecodeResult Decode(string jwt , string secretKey);
    }
}