using System.Collections.Generic;
using System.Net.Http;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;

namespace Fingo.Auth.AuthServer.Client.Services.Implementation
{
    public class PostService : IPostService
    {
        public string SendAndGetAnswer(string adress , Dictionary<string , string> parameters)
        {
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            handler.ServerCertificateCustomValidationCallback += (message , certificate , chain , errors) => true;
            var client = new HttpClient(handler);

            var content = new FormUrlEncodedContent(parameters);
            var response = client.PostAsync(adress , content).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }
    }
}