using System;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Newtonsoft.Json;

namespace Fingo.Auth.ManagementApp.Services.Implementation
{
    public class TokenService : ITokenService
    {
        public string RegistrationState(string jwt)
        {
            try
            {
                var payloadBase64 = jwt.Split('.')[1];

                while (payloadBase64.Length % 4 != 0)
                    payloadBase64 += "=";

                var payloadJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payloadBase64));
                var type = new {registration_state = ""};
                var payload = JsonConvert.DeserializeAnonymousType(payloadJson, type);

                if (payload.registration_state == "registered" || payload.registration_state == "to_be_confirmed")
                    return payload.registration_state;

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
