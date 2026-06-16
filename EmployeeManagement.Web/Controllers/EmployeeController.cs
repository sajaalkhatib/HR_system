using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Web.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    // GET: Employee
    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return View(employees);
    }

    // GET: Employee/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsDropdownAsync();
        return View();
    }

    // POST: Employee/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeDto createEmployeeDto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsDropdownAsync();
            return View(createEmployeeDto);
        }

        try
        {
            await _employeeService.CreateEmployeeAsync(createEmployeeDto);
            TempData["Success"] = "Employee created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateDepartmentsDropdownAsync();
            return View(createEmployeeDto);
        }
    }

    // GET: Employee/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        var updateEmployeeDto = new UpdateEmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = employee.FullName,
            Email = employee.Email,
            MobileNumber = employee.MobileNumber,
            DepartmentId = employee.DepartmentId,
            JobTitle = employee.JobTitle,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        };

        await PopulateDepartmentsDropdownAsync(updateEmployeeDto.DepartmentId);
        return View(updateEmployeeDto);
    }

    // POST: Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        if (id != updateEmployeeDto.EmployeeId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsDropdownAsync(updateEmployeeDto.DepartmentId);
            return View(updateEmployeeDto);
        }

        try
        {
            await _employeeService.UpdateEmployeeAsync(updateEmployeeDto);
            TempData["Success"] = "Employee updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateDepartmentsDropdownAsync(updateEmployeeDto.DepartmentId);
            return View(updateEmployeeDto);
        }
    }

    // POST: Employee/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Success"] = "Employee deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Employee/Search
    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return RedirectToAction(nameof(Index));
        }

        var employees = await _employeeService.SearchEmployeesAsync(searchTerm);
        ViewBag.SearchTerm = searchTerm;
        return View("Index", employees);
    }

    // GET: Employee/SearchByDepartment
    public async Task<IActionResult> SearchByDepartment(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            return RedirectToAction(nameof(Index));
        }

        var employees = await _employeeService.SearchByDepartmentAsync(departmentName);
        ViewBag.SearchTerm = departmentName;
        return View("Index", employees);
    }

    private async Task PopulateDepartmentsDropdownAsync(object? selectedDepartment = null)
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", selectedDepartment);
    }
}