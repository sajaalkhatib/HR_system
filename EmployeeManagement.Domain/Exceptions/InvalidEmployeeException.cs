namespace EmployeeManagement.Domain.Exceptions;

public class InvalidEmployeeException : DomainException
{
    public InvalidEmployeeException(string message) : base(message)
    {
    }

    public InvalidEmployeeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}