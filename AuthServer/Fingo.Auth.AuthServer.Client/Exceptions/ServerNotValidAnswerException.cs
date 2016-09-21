using System;

namespace Fingo.Auth.AuthServer.Client.Exceptions
{
    public class ServerNotValidAnswerException : Exception
    {
        public new const string Message =
            "The authentication server returned invalid answer. Try again in a few seconds.";
    }
}