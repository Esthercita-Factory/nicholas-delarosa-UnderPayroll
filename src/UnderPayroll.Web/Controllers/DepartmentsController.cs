using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var department =
            await _departmentService.GetDetailsAsync(id);

        if (department is null)
        {
            return NotFound();
        }

        return View(department);
    }

    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!await _departmentService.DeleteAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] =
            "El departamento se eliminó correctamente.";

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
}
