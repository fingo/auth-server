using System;
using System.Collections.Generic;
using Fingo.Auth.AuthServer.Services.Interfaces;
using Jwt;

namespace Fingo.Auth.AuthServer.Services.Implementation
{
    public class JwtLibraryWrapperService : IJwtLibraryWrapperService
    {
        public string Encode(Dictionary<string , object> payload , string secretKey)
        {
            return JsonWebToken.Encode(payload , secretKey , JwtHashAlgorithm.HS256);
        }

        public DecodeResult Decode(string jwt , string secretKey)
        {
            try
            {
                JsonWebToken.DecodeToObject<Dictionary<string , object>>(jwt , secretKey);
            }
            catch (Exception e)
            {
                if (e.Message == "Token has expired.")
                    return DecodeResult.TokenExpired;

                return DecodeResult.TokenInvalid;
            }

            return DecodeResult.TokenValid;
        }
    }
}