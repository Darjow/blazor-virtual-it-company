using BCrypt.Net;
using System;
using System.Linq;
using System.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace Domain.Utility
{
    public static class PasswordUtilities
    {
        public static string HashPassword(string password)
        {
           return BC.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
              

        public static string GeneratePassword(int length, int amount_upper, int amount_lower, int amount_numbers, int amount_symbols)
        {
            string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lower = upper.ToLower();
            string numbers = "0123456789";
            string symbols = "?=.*?[#?!@$%^&*-]";
            string all = upper + lower + numbers + symbols;

            string randomString(int amount, string source) =>
                new string(Enumerable.Repeat(source, amount).Select(s => s[RandomNumberGenerator.GetInt32(s.Length)]).ToArray());

            string password = randomString(amount_upper, upper) +
                                randomString(amount_lower, lower) +
                                randomString(amount_numbers, numbers) +
                                randomString(amount_symbols, symbols) +
                                randomString(length - amount_upper - amount_lower - amount_numbers - amount_symbols, all);

            return new string(password.ToCharArray().OrderBy(e => (new Random().Next(2) % 2) == 0).ToArray());
        }



    }
}
