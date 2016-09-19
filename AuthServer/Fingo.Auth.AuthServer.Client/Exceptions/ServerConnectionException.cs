using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class ServerConnectionException : Exception
    {
        public new const string Message = "Unable to connect to the authentication server. Try again in a few seconds.";
    }
}