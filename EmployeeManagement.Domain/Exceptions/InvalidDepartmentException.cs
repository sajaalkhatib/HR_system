namespace EmployeeManagement.Domain.Exceptions;

public class InvalidDepartmentException : DomainException
{
    public InvalidDepartmentException(string message) : base(message)
    {
    }

    public InvalidDepartmentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}