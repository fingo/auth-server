using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class PasswordNotSetException : Exception
    {
        public new const string Message = "Server was unable to set password because of some internal error.";
    }
}