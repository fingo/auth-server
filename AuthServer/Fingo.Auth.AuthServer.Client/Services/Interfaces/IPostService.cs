using System.Collections.Generic;

namespace Fingo.Auth.AuthServer.Client.Services.Interfaces
{
    public interface IPostService
    {
        string SendAndGetAnswer(string adress, Dictionary<string, string> parameters);
    }
}
