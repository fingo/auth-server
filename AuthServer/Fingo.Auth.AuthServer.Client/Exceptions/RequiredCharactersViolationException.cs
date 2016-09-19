using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class RequiredCharactersViolationException : Exception
    {
        public new const string Message = "Password violates \"required characters\" policy.";
    }
}