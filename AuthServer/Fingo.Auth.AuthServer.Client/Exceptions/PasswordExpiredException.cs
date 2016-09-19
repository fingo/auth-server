using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class PasswordExpiredException : Exception
    {
        public new const string Message = "Your password has expired.";
    }
}
