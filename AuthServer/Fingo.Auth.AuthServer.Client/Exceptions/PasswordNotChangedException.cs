using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class PasswordNotChangedException : Exception
    {
        public new const string Message = "Your password wasn't changed.";
    }
}