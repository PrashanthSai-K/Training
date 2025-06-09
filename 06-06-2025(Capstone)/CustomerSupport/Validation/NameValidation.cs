using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupport.Validation;

public class NameValidation : ValidationAttribute
{
    public NameValidation()
    {
        ErrorMessage = "Name must contain only letters and spaces.";
    }

    public override bool IsValid(object? value)
    {
        var name = value as string;
        if (string.IsNullOrEmpty(name))
            return false;
        foreach (var item in name)
        {
            if (!char.IsLetter(item) && !char.IsWhiteSpace(item))
                return false;
        }
        return true;
    }
}
