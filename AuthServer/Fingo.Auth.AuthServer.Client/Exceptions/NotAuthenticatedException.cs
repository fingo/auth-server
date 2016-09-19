using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public new const string Message = "Wrong e-mail or password.";
    }
}