using System;

namespace WholeApplication.Exceptions;

public class CollectionEmptyException : Exception
{
    private string _message = "Collection Empty";
    public CollectionEmptyException(string message)
    {
        _message = message;
    }
    public override string Message => _message;
}
