namespace EmployeeManagement.Domain.Entities;

public class Department
{
    private readonly List<Employee> _employees = new();

    public int DepartmentId { get; set; }
    public string DepartmentName { get; private set; } = string.Empty;
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Department() { }

    public Department(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
            throw new Exceptions.InvalidDepartmentException("Department name cannot be empty.");

        if (departmentName.Length > 100)
            throw new Exceptions.InvalidDepartmentException("Department name cannot exceed 100 characters.");

        DepartmentName = departmentName;
    }

    public Department(int departmentId, string departmentName) : this(departmentName)
    {
        DepartmentId = departmentId;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new Exceptions.InvalidDepartmentException("Department name cannot be empty.");

        if (newName.Length > 100)
            throw new Exceptions.InvalidDepartmentException("Department name cannot exceed 100 characters.");

        DepartmentName = newName;
    }
}