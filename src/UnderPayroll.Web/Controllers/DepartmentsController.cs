using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using UnderPayroll.Web.Data;
// using UnderPayroll.Web.Models;
using UnderPayroll.Web.Services;
using UnderPayroll.Web.ViewModels.Departments;

namespace UnderPayroll.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(bool soloActivos = false)
    {
        ViewData["SoloActivos"] = soloActivos;
        
        var datos = await _departmentService.GetAllAsync(soloActivos);
        
        return View(datos);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var departamento = await _departmentService.GetDetailsAsync(id);

        if (departamento is null)
        {
            return NotFound();
        }
        
        return View(departamento);
    }

    public IActionResult Create()
    {
        return View(new DepartmentFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentFormViewModel modelo)
    {
        await ValidarDuplicadosAsync(modelo);

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }
        
        await _departmentService.CreateAsync(modelo);

        TempData["Message"] = $"El departamento {modelo.Name} se creó correctamente.";
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var modelo = await _departmentService.GetForEditAsync(id);

        if (modelo is null)
        {
            return NotFound();
        }
        
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, DepartmentFormViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }
        
        await ValidarDuplicadosAsync(modelo);

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        if (!await _departmentService.UpdateAsync(modelo))
        {
            return NotFound();
        }

        TempData["Mensaje"] = $"El departamento {modelo.Name} se actualizó correctamente.";
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Deactivate(Guid id)
    {
        var departamento = await _departmentService.GetDetailsAsync(id);

        if (departamento is null)
        {
            return NotFound();
        }
        
        return View(departamento);
    }

    [HttpPost]
    [ActionName(nameof(Deactivate))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(Guid id)
    {
        if (!await _departmentService.DeactivateAsync(id))
        {
            return NotFound();
        }
        
        TempData["Mensaje"] = "El departamento se desactivó correctamente.";
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        if (!await _departmentService.ActivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] = "El departamento se reactivó correctamente.";
        
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarDuplicadosAsync(DepartmentFormViewModel modelo)
    {
        var excluirId = modelo.Id == Guid.Empty ? (Guid?)null : modelo.Id;

        if (!string.IsNullOrWhiteSpace(modelo.Code) && await _departmentService.CodeExistsAsync(modelo.Code, excluirId))
        {
            ModelState.AddModelError(nameof(modelo.Code), "Ya existe un departamento con ese código");
        }

        if (!string.IsNullOrWhiteSpace(modelo.Name) && await _departmentService.NameExistsAsync(modelo.Name, excluirId))
        {
            ModelState.AddModelError(nameof(modelo.Name), "Ya existe un departamento con ese nombre.");
        }
    }
    
    // private readonly ApplicationDbContext _baseDeDatos;
    //
    // public DepartmentsController(ApplicationDbContext context)
    // {
    //     _baseDeDatos = context;
    // }
    //
    // // READ
    // public async Task<IActionResult> Index()
    // {
    //     var departments = await _baseDeDatos.Departments
    //         .OrderBy(d => d.Name)
    //         .ToListAsync();
    //     
    //     return View(departments);
    // }
    //
    // // DETAILS
    // public async Task<IActionResult> Details(Guid? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     var department = await _baseDeDatos.Departments
    //         .FirstOrDefaultAsync(d => d.Id == id);
    //
    //     if (department == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return View(department);
    // }
    //
    // // CREATE - GET
    // [HttpGet]
    // public IActionResult Create()
    // {
    //     return View();
    // }
    //
    // // CREATE - POST
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create(Department department)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         department.Id = Guid.NewGuid();
    //         department.CreatedAt = DateTime.UtcNow;
    //         // department.UpdatedAt = DateTime.UtcNow;
    //         
    //         _baseDeDatos.Departments.Add(department);
    //         await _baseDeDatos.SaveChangesAsync();
    //
    //         return RedirectToAction(nameof(Index));
    //     }
    //     return View(department);
    // }
    //
    // // EDIT - GET
    // public async Task<IActionResult> Edit(Guid? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var department = await _baseDeDatos.Departments.FindAsync(id);
    //
    //     if (department == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return View(department);
    // }
    //
    // // EDIT - POST
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(Guid id, Department department)
    // {
    //     if (id != department.Id)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (ModelState.IsValid)
    //     {
    //         var existingDepartment = await _baseDeDatos.Departments
    //             .FindAsync(id);
    //
    //         if (existingDepartment == null)
    //         {
    //             return NotFound();
    //         }
    //         
    //         existingDepartment.Code = department.Code;
    //         existingDepartment.Name = department.Name;
    //         existingDepartment.Description = department.Description;
    //         existingDepartment.Location = department.Location;
    //         existingDepartment.Budget = department.Budget;
    //         existingDepartment.Phone = department.Phone;
    //         existingDepartment.Email = department.Email;
    //         existingDepartment.IsActive = department.IsActive;
    //         existingDepartment.UpdatedAt = DateTime.UtcNow;
    //         
    //         await _baseDeDatos.SaveChangesAsync();
    //         
    //         return RedirectToAction(nameof(Index));
    //     }
    //     
    //     return View(department);
    // }
    //
    // // DELETE - GET
    // public async Task<IActionResult> Delete(Guid? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var department = await _baseDeDatos.Departments
    //         .FirstOrDefaultAsync(d => d.Id == id);
    //
    //     if (department == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return View(department);
    // }
    //
    // // DELETE - POST
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(Guid id)
    // {
    //     var department = await _baseDeDatos.Departments
    //         .FindAsync(id);
    //
    //     if (department != null)
    //     {
    //         _baseDeDatos.Departments.Remove(department);
    //         await _baseDeDatos.SaveChangesAsync();
    //     }
    //     
    //     return RedirectToAction(nameof(Index));
    // }
}
