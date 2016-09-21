using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class AccountExpiredException : Exception
    {
        public new const string Message = "Your account has expired.";
    }
}