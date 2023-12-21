using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Konscious.Security.Cryptography;

namespace ProjectHorizon.Shared.Library.Helper
{
    public class UtilityHelper
    {
        //use this method to check if the input is a valid SHA512 hash
        public static bool IsSHA512Hash(string input)
        {
            if (input.Length != 128) { return false; }

            try
            {
                byte[] hashBytes = StringToByteArray(input);
                if (hashBytes.Length != 64)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
        //--//

        //use this method to generate a random salt
        //only use this when using SHA512 or when you need a random number
        public static byte[] GenerateRandomByte(int size = 32)
        {
            byte[] salt = new byte[size];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }

            return salt;
        }
        //--//

        public static string GenerateRandomString(int size = 128)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                int randomIndex = random.Next(0, chars.Length);
                stringBuilder.Append(chars[randomIndex]);
            }

            return stringBuilder.ToString();
        }

        //use this method to generate a random salt via BCrypt
        //you can use this when using SHA512, BCrypt or when you need a random salt
        public static string GenerateSaltBCrypt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        //use this method to generate a SHA256 hash from a string input
        //the result is a byte array with 2 elements
        //example :
        //byte[][] result = UtilityHelper.GenerateSHA256Hash(input);
        //byte[] hashedValue = result[0];
        //byte[] salt = result[1];
        public static byte[][] GenerateSHA512Hash(string inputString)
        {
            byte[] salt = GenerateRandomByte();

            byte[] saltedValue = Encoding.UTF8.GetBytes(inputString + Convert.ToBase64String(salt));
            byte[] hashedBytes = SHA512.HashData(saltedValue);

            byte[][] result = [hashedBytes, salt];
            return result;
        }
        //--//

        //use this method to generate a BCrypt hash from a string input
        //the result is a string array with 2 elements
        //example :
        //string[][] result = UtilityHelper.GenerateBCryptHash(input);
        //string hashedValue = result[0];
        //string salt = result[1];
        public static string[][] GenerateBCryptHash(string inputString)
        {
            string salt = GenerateSaltBCrypt();

            string hashedValue = BCrypt.Net.BCrypt.HashPassword(inputString, salt);

            string[][] result = [[hashedValue], [salt]];
            return result;
        }
        //--//

        //use this method to verify an un-encrypted string input with a BCrypt hash
        public static bool VerifyBCryptHash(string inputString, string hashedValue)
        {
            return BCrypt.Net.BCrypt.Verify(inputString, hashedValue);
        }
        //--//

        //use this method to generate an Argon2 hash from a string input
        //the result is a byte array with 2 elements
        //example :
        //byte[][] result = UtilityHelper.GenerateArgon2Hash(input);
        //byte[] hashedValue = result[0];
        //byte[] salt = result[1];
        public static byte[][] GenerateArgon2Hash(string inputString)
        {
            byte[] salt = GenerateRandomByte();
            byte[] associateddata = Encoding.UTF8.GetBytes(inputString);
            byte[] saltedValue = Encoding.UTF8.GetBytes(inputString + salt);

            using (var argon2 = new Argon2id(saltedValue))
            {
                argon2.DegreeOfParallelism = 8; //probably should be higher
                argon2.Iterations = 4; //probably should be higher
                argon2.Salt = salt;
                argon2.MemorySize = 1024 * 1024;
                argon2.AssociatedData = associateddata;
                //argon2.KnownSecret = Encoding.UTF8.GetBytes("ProjectHorizon"); //this need to be created later

                byte[][] result = [argon2.GetBytes(128), salt];
                return result;
            }
        }

        //use this method to convert input string to Argon2 hash
        //usually used to store password in database, since database has to store the converted hash
        public static byte[] ConvertArgon2Hash(string inputString, byte[] inputSalt)
        {
            //byte[] salt = Encoding.UTF8.GetBytes(inputSalt);
            byte[] associateddata = Encoding.UTF8.GetBytes(inputString);
            byte[] saltedValue = Encoding.UTF8.GetBytes(inputString + inputSalt);

            using (var argon2 = new Argon2id(saltedValue))
            {
                argon2.DegreeOfParallelism = 8; //probably should be higher
                argon2.Iterations = 4; //probably should be higher
                argon2.Salt = inputSalt;
                argon2.MemorySize = 1024 * 1024;
                argon2.AssociatedData = associateddata;
                //argon2.KnownSecret = Encoding.UTF8.GetBytes("ProjectHorizon"); //this need to be created later

                return argon2.GetBytes(128);
            }
        }

    }
}
