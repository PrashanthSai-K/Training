using System;

namespace CustomerSupport.Exceptions;

public class DuplicateEntryException : Exception
{
    public DuplicateEntryException(string message) : base(message)
    {
    }
}
