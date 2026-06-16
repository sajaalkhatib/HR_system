using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto);
    Task DeleteEmployeeAsync(int employeeId);
    Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm);
    Task<IEnumerable<EmployeeDto>> SearchByDepartmentAsync(string departmentName);
    Task ActivateEmployeeAsync(int employeeId);
    Task DeactivateEmployeeAsync(int employeeId);
}