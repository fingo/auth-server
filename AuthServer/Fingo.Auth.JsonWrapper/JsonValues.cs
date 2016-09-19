namespace Fingo.Auth.JsonWrapper
{
    public static class JsonValues
    {
        public const string PasswordSet = "password_set";
        public const string PasswordExpired = "password_expired";
        public const string AccountExpired = "account_expired";
        public const string RequiredCharactersViolation = "password_violates_required_characters_constraint";
        public const string PasswordLengthIncorrect = "password_length_incorrect";
        public const string PasswordTooCommon = "password_too_common";
        public const string UserCreatedInProject = "user_created_in_project";
        public const string UsersPasswordChanged = "users_password_changed";
        public const string Authenticated = "authenticated";
        public const string NotAuthenticated = "not_authenticated";
        public const string WrongAccessToken = "wrong_access_token";
        public const string TokenValid = "token_valid";
        public const string TokenInvalid = "token_invalid";
        public const string TokenExpired = "token_expired";
        public const string Error = "error";
    }
}
