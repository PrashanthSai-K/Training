using System;

namespace CustomerSupport.Exceptions;

public class EntityEmptyException : Exception
{
    public EntityEmptyException(string message) : base(message) { }
}
