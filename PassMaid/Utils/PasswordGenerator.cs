using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.Utils
{
    public static class PasswordGenerator
    {
        // Available characters for password generation
        private const string LOWERCASE_CHARS = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPERCASE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBER_CHARS = "0123456789";
        private const string SPECIAL_CHARS = "!@#$%^&*";

        // Min and max password length
        private const int MIN_LENGTH = 8;
        private const int MAX_LENGTH = 128;

        public static string GeneratePassword(int length, bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial)
        {
            if (length < MIN_LENGTH || length > MAX_LENGTH)
            {
                return $"Password must be between {MIN_LENGTH} and {MAX_LENGTH} characters!";
            }

            string chars = "";

            if (includeLowercase)
            {
                chars += LOWERCASE_CHARS;
            }

            if (includeUppercase)
            {
                chars += UPPERCASE_CHARS;
            }

            if (includeNumeric)
            {
                chars += NUMBER_CHARS;
            }

            if (includeSpecial)
            {
                chars += SPECIAL_CHARS;
            }
            
            if (String.IsNullOrEmpty(chars))
            {
                return "You must toggle at least one character type!";
            }

            StringBuilder sb = new StringBuilder();
            Random r = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[r.Next(chars.Length - 1)]);
            }           

            return sb.ToString();
        }
    }
}
