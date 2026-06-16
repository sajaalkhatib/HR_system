using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        return employee is null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        var department = await _departmentRepository.GetByIdAsync(createEmployeeDto.DepartmentId);
        if (department is null)
            throw new KeyNotFoundException($"Department with ID {createEmployeeDto.DepartmentId} not found.");

        // Check email uniqueness
        var existingEmployee = await _employeeRepository.GetByEmailAsync(createEmployeeDto.Email);
        if (existingEmployee is not null)
            throw new InvalidEmployeeException($"An employee with email '{createEmployeeDto.Email}' already exists.");

        var employee = new Employee(
            createEmployeeDto.FullName,
            createEmployeeDto.Email,
            createEmployeeDto.MobileNumber,
            createEmployeeDto.DepartmentId,
            createEmployeeDto.JobTitle,
            createEmployeeDto.HireDate);

        var created = await _employeeRepository.AddAsync(employee);
        return MapToDto(created);
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _employeeRepository.GetByIdAsync(updateEmployeeDto.EmployeeId);
        if (employee is null)
            throw new KeyNotFoundException($"Employee with ID {updateEmployeeDto.EmployeeId} not found.");

        var department = await _departmentRepository.GetByIdAsync(updateEmployeeDto.DepartmentId);
        if (department is null)
            throw new KeyNotFoundException($"Department with ID {updateEmployeeDto.DepartmentId} not found.");

        // Check email uniqueness (exclude current employee)
        var existingEmployee = await _employeeRepository.GetByEmailAsync(updateEmployeeDto.Email);
        if (existingEmployee is not null && existingEmployee.EmployeeId != updateEmployeeDto.EmployeeId)
            throw new InvalidEmployeeException($"An employee with email '{updateEmployeeDto.Email}' already exists.");

        employee.UpdateDetails(
            updateEmployeeDto.FullName,
            updateEmployeeDto.Email,
            updateEmployeeDto.MobileNumber,
            updateEmployeeDto.DepartmentId,
            updateEmployeeDto.JobTitle,
            updateEmployeeDto.HireDate);

        if (updateEmployeeDto.IsActive && !employee.IsActive)
            employee.Activate();
        else if (!updateEmployeeDto.IsActive && employee.IsActive)
            employee.Deactivate();

        await _employeeRepository.UpdateAsync(employee);
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee is null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        await _employeeRepository.DeleteAsync(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm)
    {
        var employees = await _employeeRepository.SearchAsync(searchTerm);
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchByDepartmentAsync(string departmentName)
    {
        var employees = await _employeeRepository.SearchByDepartmentAsync(departmentName);
        return employees.Select(MapToDto);
    }

    public async Task ActivateEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee is null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        employee.Activate();
        await _employeeRepository.UpdateAsync(employee);
    }

    public async Task DeactivateEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee is null)
            throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");

        employee.Deactivate();
        await _employeeRepository.UpdateAsync(employee);
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = employee.FullName,
            Email = employee.Email,
            MobileNumber = employee.MobileNumber,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.DepartmentName ?? string.Empty,
            JobTitle = employee.JobTitle,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        };
    }
}