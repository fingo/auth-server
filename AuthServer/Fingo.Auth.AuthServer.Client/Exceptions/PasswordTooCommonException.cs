using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class PasswordTooCommonException : Exception
    {
        public new const string Message = "Your password is too common.";
    }
}