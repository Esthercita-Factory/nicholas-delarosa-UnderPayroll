using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Data;
using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly ApplicationDbContext _baseDeDatos;

    public DepartmentsController(ApplicationDbContext context)
    {
        _baseDeDatos = context;
    }
    
    // READ
    public async Task<IActionResult> Index()
    {
        var departments = await _baseDeDatos.Departments
            .OrderBy(d => d.Name)
            .ToListAsync();
        
        return View(departments);
    }
    
    // DETAILS
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var department = await _baseDeDatos.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return NotFound();
        }
        
        return View(department);
    }
    
    // CREATE - GET
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        if (ModelState.IsValid)
        {
            department.Id = Guid.NewGuid();
            department.CreatedAt = DateTime.UtcNow;
            // department.UpdatedAt = DateTime.UtcNow;
            
            _baseDeDatos.Departments.Add(department);
            await _baseDeDatos.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }
    
    // EDIT - GET
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _baseDeDatos.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound();
        }
        
        return View(department);
    }
    
    // EDIT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Department department)
    {
        if (id != department.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingDepartment = await _baseDeDatos.Departments
                .FindAsync(id);

            if (existingDepartment == null)
            {
                return NotFound();
            }
            
            existingDepartment.Code = department.Code;
            existingDepartment.Name = department.Name;
            existingDepartment.Description = department.Description;
            existingDepartment.Location = department.Location;
            existingDepartment.Budget = department.Budget;
            existingDepartment.Phone = department.Phone;
            existingDepartment.Email = department.Email;
            existingDepartment.IsActive = department.IsActive;
            existingDepartment.UpdatedAt = DateTime.UtcNow;
            
            await _baseDeDatos.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        return View(department);
    }
    
    // DELETE - GET
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _baseDeDatos.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return NotFound();
        }
        
        return View(department);
    }
    
    // DELETE - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var department = await _baseDeDatos.Departments
            .FindAsync(id);

        if (department != null)
        {
            _baseDeDatos.Departments.Remove(department);
            await _baseDeDatos.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }
}
