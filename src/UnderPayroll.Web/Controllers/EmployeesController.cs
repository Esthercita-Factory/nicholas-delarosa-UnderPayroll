using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnderPayroll.Web.Services;
using UnderPayroll.Web.ViewModels.Employees;

namespace UnderPayroll.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeesController(
        IEmployeeService employeeService,
        IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(
        bool? activo = null,
        string? busqueda = null)
    {
        ViewData["Activo"] = activo;
        ViewData["Busqueda"] = busqueda;

        var employees =
            await _employeeService.GetAllAsync(activo, busqueda);

        return View(employees);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var employee =
            await _employeeService.GetDetailsAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var modelo = new EmployeeFormViewModel();

        await LoadDepartments();

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        EmployeeFormViewModel modelo)
    {
        await ValidateDuplicatesAsync(modelo);

        if (!ModelState.IsValid)
        {
            await LoadDepartments(modelo.DepartmentId);
            return View(modelo);
        }

        await _employeeService.CreateAsync(modelo);

        TempData["Mensaje"] =
            $"El empleado {modelo.FirstName} {modelo.LastName} se creó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var modelo =
            await _employeeService.GetForEditAsync(id);

        if (modelo is null)
        {
            return NotFound();
        }

        await LoadDepartments(modelo.DepartmentId);

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        EmployeeFormViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        await ValidateDuplicatesAsync(modelo);

        if (!ModelState.IsValid)
        {
            await LoadDepartments(modelo.DepartmentId);
            return View(modelo);
        }

        if (!await _employeeService.UpdateAsync(modelo))
        {
            return NotFound();
        }

        TempData["Mensaje"] =
            $"El empleado {modelo.FirstName} {modelo.LastName} se actualizó correctamente.";

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var employee =
            await _employeeService.GetDetailsAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!await _employeeService.DeleteAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] =
            "El empleado se eliminó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var employee =
            await _employeeService.GetDetailsAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [ActionName(nameof(Deactivate))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateConfirmed(
        Guid id)
    {
        if (!await _employeeService.DeactivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] =
            "El empleado se desactivó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        if (!await _employeeService.ActivateAsync(id))
        {
            return NotFound();
        }

        TempData["Mensaje"] =
            "El empleado se reactivó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateDuplicatesAsync(
        EmployeeFormViewModel modelo)
    {
        var excluirId =
            modelo.Id == Guid.Empty
                ? (Guid?)null
                : modelo.Id;

        if (!string.IsNullOrWhiteSpace(modelo.Document) &&
            await _employeeService.DocumentExistsAsync(
                modelo.Document,
                excluirId))
        {
            ModelState.AddModelError(
                nameof(modelo.Document),
                "Ya existe un empleado con ese documento.");
        }

        if (!string.IsNullOrWhiteSpace(modelo.Email) &&
            await _employeeService.EmailExistsAsync(
                modelo.Email,
                excluirId))
        {
            ModelState.AddModelError(
                nameof(modelo.Email),
                "Ya existe un empleado con ese correo.");
        }
    }

    private async Task LoadDepartments(
        Guid? selectedDepartment = null)
    {
        var departments =
            await _departmentService.GetAllAsync(true);

        ViewBag.Departments = new SelectList(
            departments,
            "Id",
            "Name",
            selectedDepartment);
    }
}