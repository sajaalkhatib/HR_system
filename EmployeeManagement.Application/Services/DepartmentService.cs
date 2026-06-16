using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(MapToDto);
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        return department is null ? null : MapToDto(department);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = new Department(createDepartmentDto.DepartmentName);
        var created = await _departmentRepository.AddAsync(department);
        return MapToDto(created);
    }

    public async Task DeleteDepartmentAsync(int departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        if (department is null)
            throw new KeyNotFoundException($"Department with ID {departmentId} not found.");

        await _departmentRepository.DeleteAsync(department);
    }

    private static DepartmentDto MapToDto(Department department)
    {
        return new DepartmentDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            EmployeeCount = department.Employees.Count
        };
    }
}