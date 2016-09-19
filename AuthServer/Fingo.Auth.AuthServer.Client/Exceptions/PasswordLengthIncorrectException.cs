using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class PasswordLengthIncorrectException : Exception
    {
        public new const string Message = "Password is too short.";
    }
}