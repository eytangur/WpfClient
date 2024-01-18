using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClient
{
    public class ValidUserName : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string symbols = "#?$!-_~";
                string text = value.ToString();
                Regex letter = new Regex("[a-zA-z]");
                if (text.Length < 5)
                    throw new Exception("At least 5 letters...");
                if (text.Length > 12)
                    throw new Exception("UserName too long");
                if (text.IndexOf(" ") != -1)
                    throw new Exception("UserName can not include space");
                if (!letter.IsMatch(text))
                {
                    return new ValidationResult(false, "Username must contain a letter");
                }

                for (int i = 0; i < text.Length; i++)
                {
                    if (!Char.IsLetter(text[i]) && !Char.IsDigit(text[i]) && symbols.IndexOf(text[i]) == -1)
                    {
                        throw new Exception($"Enter only digit, letter and [{symbols}]");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class ValidPassword : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string passwsord = value as string;
                Regex digit = new Regex("[0-9]");
                Regex uppercase = new Regex("[A-Z]");

                if (passwsord.Length < 8)
                {
                    return new ValidationResult(false, "password must be at least 8 chars"); // check if the password lenght is 8
                }
                else if (!digit.IsMatch(passwsord))
                {
                    return new ValidationResult(false, "password must contain digit"); // search lowercase letter
                }
                else if (!uppercase.IsMatch(passwsord))
                {
                    return new ValidationResult(false, "password must contain uppercase letter"); // search uppercase letter
                }               
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }

            return ValidationResult.ValidResult;
           
        }
    }
    public class ValidPhone : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string text = value.ToString();
                for (int i = 0; i < text.Length; i++)
                {
                    if (!Char.IsDigit(text[i]))
                    {
                        return new ValidationResult(false, "only digits...");
                    }
                }
                if (text.Length != 10)
                    return new ValidationResult(false, "Phone must be at 10 numbers!");
                if (text.IndexOf(" ") != -1)
                    return new ValidationResult(false, "Phone must not include space");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class ValidBDay : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null)
                {
                    return new ValidationResult(false, "Selection is required.");
                }

                DateTime date=DateTime.Parse(value.ToString());
                if(date > DateTime.Today.AddYears(-15))
                    return new ValidationResult(false, "You'r too young!");
                if (date < DateTime.Today.AddYears(-120))
                    return new ValidationResult(false, "You'r still alive??");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class ValidEmail : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string email = value as string;
                Regex emailRegex = new Regex("([A-Za-z0-9]+[._-])*[A-Za-z0-9]+@[A-Za-z0-9-]+(\\.[A-Z|a-z]{2,})+");
                Match match = emailRegex.Match(email);

                if (!match.Success)
                {
                    return new ValidationResult(false, "enter a real email!");
                }

            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }

            return ValidationResult.ValidResult;
        }
    }
}
