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
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NumericChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*";

        // Min and max password length
        private const int MinLength = 8;
        private const int MaxLength = 128;

        public static string GeneratePassword(int length, bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial)
        {
            if (length < MinLength || length > MaxLength)
            {
                return $"Password must be between {MinLength} and {MaxLength} characters!";
            }

            string chars = "";

            if (includeLowercase)
            {
                chars += LowercaseChars;
            }

            if (includeUppercase)
            {
                chars += UppercaseChars;
            }

            if (includeNumeric)
            {
                chars += NumericChars;
            }

            if (includeSpecial)
            {
                chars += SpecialChars;
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
