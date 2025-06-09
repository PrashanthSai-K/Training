using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CustomerSupport.Validation;

public class PasswordValidator : ValidationAttribute
{
    public PasswordValidator()
    {
        ErrorMessage = "Password length must be 8 to 12 and should be alphanumerical";
    }

    public override bool IsValid(object? value)
    {
        var password = value as string;
        if (password == null)
            return false;

        if (password.Length < 8 || password.Length > 12)
            return false;

        return Regex.IsMatch(password, @"[a-z]") &&
               Regex.IsMatch(password, @"[A-Z]") &&
               Regex.IsMatch(password, @"[0-9]");
    } 
}
