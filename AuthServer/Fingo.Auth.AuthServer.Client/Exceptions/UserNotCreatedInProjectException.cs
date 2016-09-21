using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class UserNotCreatedInProjectException : Exception
    {
        public new const string Message = "Your account was not created because of some internal error.";
    }
}