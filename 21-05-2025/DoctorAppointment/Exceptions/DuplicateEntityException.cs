using System;

namespace DoctorAppointment.Exceptions;

public class DuplicateEntityException : Exception
{
    private string _message = "Duplicate Entity Found";
    public DuplicateEntityException(string message)
    {
        _message = message;
    }
    public override string Message => _message;
}
