using UnderPayroll.Web.Models;
using UnderPayroll.Web.Repositories;
using UnderPayroll.Web.ViewModels.Employees;

namespace UnderPayroll.Web.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeListItemViewModel>> GetAllAsync(
        bool soloActivos = false)
    {
        var employees =
            await _repository.GetAllAsync(soloActivos);

        return employees
            .Select(e => new EmployeeListItemViewModel
            {
                Id = e.Id,
                Document = e.Document,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position,
                Salary = e.Salary,
                HireDate = e.HireDate,
                DepartmentId = e.DepartmentId,
                DepartmentName =
                    e.Department?.Name ?? "Sin departamento",
                IsActive = e.IsActive
            })
            .ToList();
    }

    public async Task<EmployeeDetailsViewModel?> GetDetailsAsync(Guid id)
    {
        var employee =
            await _repository.GetByIdAsync(id);

        if (employee is null)
        {
            return null;
        }

        return new EmployeeDetailsViewModel
        {
            Id = employee.Id,
            Document = employee.Document,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            DepartmentName =
                employee.Department?.Name ?? "Sin departamento",
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    public async Task<EmployeeFormViewModel?> GetForEditAsync(Guid id)
    {
        var employee =
            await _repository.GetByIdAsync(id);

        if (employee is null)
        {
            return null;
        }

        return new EmployeeFormViewModel
        {
            Id = employee.Id,
            Document = employee.Document,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId
        };
    }

    public Task CreateAsync(EmployeeFormViewModel modelo)
    {
        var employee = new Employee
        {
            IsActive = true,
            UpdatedAt = null
        };

        ApplyChanges(modelo, employee);

        return _repository.AddAsync(employee);
    }

    public async Task<bool> UpdateAsync(
        EmployeeFormViewModel modelo)
    {
        var employee =
            await _repository.GetByIdAsync(modelo.Id);

        if (employee is null)
        {
            return false;
        }

        ApplyChanges(modelo, employee);

        employee.UpdatedAt =
            DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(employee);

        return true;
    }
    
    public Task<bool> DeleteAsync(Guid id)
    {
        return _repository.DeleteAsync(id);
    }

    public Task<bool> DeactivateAsync(Guid id)
    {
        return ChangeStatusAsync(id, false);
    }

    public Task<bool> ActivateAsync(Guid id)
    {
        return ChangeStatusAsync(id, true);
    }

    public Task<bool> DocumentExistsAsync(
        string document,
        Guid? excluirId = null)
    {
        return _repository.DocumentExistsAsync(
            document.Trim(),
            excluirId);
    }

    public Task<bool> EmailExistsAsync(
        string email,
        Guid? excluirId = null)
    {
        return _repository.EmailExistsAsync(
            email.Trim().ToLowerInvariant(),
            excluirId);
    }

    private async Task<bool> ChangeStatusAsync(
        Guid id,
        bool activo)
    {
        var employee =
            await _repository.GetByIdAsync(id);

        if (employee is null)
        {
            return false;
        }

        employee.IsActive = activo;
        employee.UpdatedAt =
            DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(employee);

        return true;
    }

    private static void ApplyChanges(
        EmployeeFormViewModel modelo,
        Employee employee)
    {
        employee.Document =
            modelo.Document.Trim();

        employee.FirstName =
            modelo.FirstName.Trim();

        employee.LastName =
            modelo.LastName.Trim();

        employee.Email =
            modelo.Email.Trim().ToLowerInvariant();

        employee.Phone =
            CleanOptional(modelo.Phone);

        employee.Position =
            modelo.Position.Trim();

        employee.Salary =
            modelo.Salary;

        if (modelo.HireDate.HasValue)
        {
            employee.HireDate = modelo.HireDate.Value;
        }

        if (modelo.DepartmentId.HasValue)
        {
            employee.DepartmentId =
                modelo.DepartmentId.Value;
        }
    }

    private static string? CleanOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}