using System;

namespace Domain.Utility
{
    public class PropertyValidator
    {

        public static bool IsValidEmail(string mail)
        {
            if (mail == null) throw new ArgumentException("Email can't be null");
            if (!mail.Contains("@")) throw new ArgumentException("Invalid Email");
            if (mail.Split("@")[0].Length == 0) throw new ArgumentException("Email has no prefix");
            if (mail.Split("@")[1].Length == 0) throw new ArgumentException("Email has nu suffix");
            if (!mail.Split("@")[1].Contains(".")) throw new ArgumentException("Email has no suffix");
            if (mail.Split("@")[1].Split(".")[1].Length < 2) throw new ArgumentException("Email suffix length must be 2 or larger");


            return true;
        }
        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            if (!phoneNumber.StartsWith("0")) throw new ArgumentException("Phone number must start with 0");
            if (phoneNumber.StartsWith("04") && phoneNumber.Length != 10) throw new ArgumentException("Phone number is not correct");
            if (!phoneNumber.StartsWith("04") && phoneNumber.Length != 9) throw new ArgumentException("Landline number is not correct");

            return true;
        }
    }
}