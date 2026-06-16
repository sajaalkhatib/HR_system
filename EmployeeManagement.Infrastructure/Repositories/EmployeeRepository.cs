using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var term = searchTerm.Trim().ToLower();
        return await _context.Employees
            .Include(e => e.Department)
            .AsNoTracking()
            .Where(e => e.FullName.ToLower().Contains(term) ||
                        e.Email.ToLower().Contains(term) ||
                        e.JobTitle.ToLower().Contains(term) ||
                        e.MobileNumber.Contains(term))
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> SearchByDepartmentAsync(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
            return await GetAllAsync();

        var term = departmentName.Trim().ToLower();
        return await _context.Employees
            .Include(e => e.Department)
            .AsNoTracking()
            .Where(e => e.Department != null && e.Department.DepartmentName.ToLower().Contains(term))
            .ToListAsync();
    }
}