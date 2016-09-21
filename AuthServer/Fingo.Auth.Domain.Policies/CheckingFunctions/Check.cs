using System;
using System.IO;
using System.Linq;
using Fingo.Auth.Domain.Policies.ConfigurationClasses;
using Fingo.Auth.Domain.Policies.Enums;

namespace Fingo.Auth.Domain.Policies.CheckingFunctions
{
    public static class Check
    {
        public static bool ExcludeCommonPasswords(ExcludeCommonPasswordsConfiguration config , string password)
        {
            using (var stream = File.OpenText(@"common_passwords_list.txt"))
            {
                while (!stream.EndOfStream)
                {
                    var commonPassword = stream.ReadLine();
                    if (password == commonPassword)
                        return false;
                }
            }

            return true;
        }

        public static bool AccountExpirationDate(AccountExpirationDateConfiguration config ,
            UserAccountExpirationDateConfiguration userConfig)
        {
            if (!config.IsEnabled || (userConfig.ExpirationDate == default(DateTime)))
                return true;

            return userConfig.ExpirationDate >= DateTime.UtcNow;
        }

        public static bool MinimumPasswordLength(MinimumPasswordLengthConfiguration config , string password)
        {
            return password.Length >= config.Length;
        }

        public static bool PasswordExpirationDate(PasswordExpirationDateConfiguration config ,
            DateTime lastPasswordChange)
        {
            switch (config.PasswordExpiration)
            {
                case PasswordExpiration.Year:
                    return lastPasswordChange.AddMonths(12) >= DateTime.UtcNow;
                case PasswordExpiration.HalfYear:
                    return lastPasswordChange.AddMonths(6) >= DateTime.UtcNow;
                case PasswordExpiration.Month:
                    return lastPasswordChange.AddMonths(1) >= DateTime.UtcNow;
                case PasswordExpiration.Custom:
                    if (!config.CustomValue.HasValue)
                        throw new Exception("Custom password expiration date not set.");
                    return lastPasswordChange.AddDays(config.CustomValue.Value) >= DateTime.UtcNow;
                default:
                    throw new Exception("Bad policy");
            }
        }

        public static bool RequiredPasswordCharacters(RequiredPasswordCharactersConfiguration config , string password)
        {
            return Implication(config.Digit , password.Any(char.IsDigit))
                   && Implication(config.Special , password.Any(IsSpecial))
                   && Implication(config.UpperCase , password.Any(char.IsUpper));
        }

        private static bool IsSpecial(char c)
        {
            return char.IsSymbol(c) || char.IsPunctuation(c) || char.IsSeparator(c) || char.IsWhiteSpace(c);
        }

        private static bool Implication(bool p , bool q)
        {
            return !p || q;
        }
    }
}