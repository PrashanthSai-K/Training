using System;

namespace CustomerSupport.Exceptions;

public class UnsupportedFileUploadException : Exception
{
    public UnsupportedFileUploadException(string message) : base(message)
    {
    }
}
