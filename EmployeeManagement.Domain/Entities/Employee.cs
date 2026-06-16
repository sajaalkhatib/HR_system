using EmployeeManagement.Domain.Exceptions;

namespace EmployeeManagement.Domain.Entities;

public class Employee
{
    public int EmployeeId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string MobileNumber { get; private set; } = string.Empty;
    public int DepartmentId { get; private set; }
    public string JobTitle { get; private set; } = string.Empty;
    public DateTime HireDate { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation property
    public Department? Department { get; private set; }

    private Employee() { }

    public Employee(
        string fullName,
        string email,
        string mobileNumber,
        int departmentId,
        string jobTitle,
        DateTime hireDate)
    {
        ValidateFullName(fullName);
        ValidateEmail(email);
        ValidateMobileNumber(mobileNumber);
        ValidateJobTitle(jobTitle);
        ValidateHireDate(hireDate);

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        MobileNumber = mobileNumber.Trim();
        DepartmentId = departmentId;
        JobTitle = jobTitle.Trim();
        HireDate = hireDate;
        IsActive = true;
    }

    public void UpdateDetails(
        string fullName,
        string email,
        string mobileNumber,
        int departmentId,
        string jobTitle,
        DateTime hireDate)
    {
        ValidateFullName(fullName);
        ValidateEmail(email);
        ValidateMobileNumber(mobileNumber);
        ValidateJobTitle(jobTitle);
        ValidateHireDate(hireDate);

        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        MobileNumber = mobileNumber.Trim();
        DepartmentId = departmentId;
        JobTitle = jobTitle.Trim();
        HireDate = hireDate;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidEmployeeException("Employee is already active.");

        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidEmployeeException("Employee is already inactive.");

        IsActive = false;
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new InvalidEmployeeException("Full name cannot be empty.");

        if (fullName.Trim().Length < 2)
            throw new InvalidEmployeeException("Full name must be at least 2 characters.");

        if (fullName.Trim().Length > 100)
            throw new InvalidEmployeeException("Full name cannot exceed 100 characters.");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidEmployeeException("Email cannot be empty.");

        if (!email.Contains('@') || !email.Contains('.'))
            throw new InvalidEmployeeException("Email must be a valid email address.");
    }

    private static void ValidateMobileNumber(string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new InvalidEmployeeException("Mobile number cannot be empty.");

        var digitsOnly = new string(mobileNumber.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length < 7 || digitsOnly.Length > 15)
            throw new InvalidEmployeeException("Mobile number must be between 7 and 15 digits.");
    }

    private static void ValidateJobTitle(string jobTitle)
    {
        if (string.IsNullOrWhiteSpace(jobTitle))
            throw new InvalidEmployeeException("Job title cannot be empty.");

        if (jobTitle.Trim().Length > 100)
            throw new InvalidEmployeeException("Job title cannot exceed 100 characters.");
    }

    private static void ValidateHireDate(DateTime hireDate)
    {
        if (hireDate > DateTime.UtcNow)
            throw new InvalidEmployeeException("Hire date cannot be in the future.");
    }
}