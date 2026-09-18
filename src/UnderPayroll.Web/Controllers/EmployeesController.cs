using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Data;
using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _baseDeDatos;

    public EmployeesController(ApplicationDbContext context)
    {
        _baseDeDatos = context;
    }

    // READ
    public async Task<IActionResult> Index()
    {
        var employees = await _baseDeDatos.Employees
            .Include(e => e.Department)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();

        return View(employees);
    }

    // DETAILS
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _baseDeDatos.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // CREATE - GET
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadDepartments();

        return View();
    }

    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (ModelState.IsValid)
        {
            employee.Id = Guid.NewGuid();
            employee.CreatedAt = DateTime.UtcNow;

            _baseDeDatos.Employees.Add(employee);

            await _baseDeDatos.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadDepartments(employee.DepartmentId);

        return View(employee);
    }

    // EDIT - GET
    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _baseDeDatos.Employees
            .FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        await LoadDepartments(employee.DepartmentId);

        return View(employee);
    }

    // EDIT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Employee employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingEmployee = await _baseDeDatos.Employees
                .FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.Document = employee.Document;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Position = employee.Position;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.UpdatedAt = DateTime.UtcNow;

            await _baseDeDatos.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadDepartments(employee.DepartmentId);

        return View(employee);
    }

    // DELETE - GET
    [HttpGet]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _baseDeDatos.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // DELETE - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var employee = await _baseDeDatos.Employees
            .FindAsync(id);

        if (employee != null)
        {
            _baseDeDatos.Employees.Remove(employee);

            await _baseDeDatos.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Cargar departamentos para el select
    private async Task LoadDepartments(Guid? selectedDepartment = null)
    {
        var departments = await _baseDeDatos.Departments
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();

        ViewBag.Departments = new SelectList(
            departments,
            "Id",
            "Name",
            selectedDepartment
        );
    }
}