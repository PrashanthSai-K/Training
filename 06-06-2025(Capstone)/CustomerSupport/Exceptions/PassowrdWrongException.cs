using System;

namespace CustomerSupport.Exceptions;

public class PassowrdWrongException : Exception
{
    public PassowrdWrongException(string message) : base(message)
    {
    }
}
