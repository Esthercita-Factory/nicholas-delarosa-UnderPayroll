using UnderPayroll.Web.Models;
using UnderPayroll.Web.Repositories;
using UnderPayroll.Web.ViewModels.Departments;

namespace UnderPayroll.Web.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentListItemViewModel>> GetAllAsync(bool soloActivos = false)
    {
        var departments = await _repository.GetAllAsync(soloActivos);

        return departments
            .Select(ToListItemViewModel)
            .ToList();
    }

    public async Task<DepartmentDetailsViewModel?> GetDetailsAsync(Guid id)
    {
        var department = await _repository.GetByIdAsync(id);

        return department is null ? null : ToDetailsViewModel(department);
    }

    public async Task<DepartmentFormViewModel?> GetForEditAsync(Guid id)
    {
        var department = await _repository.GetByIdAsync(id);

        return department is null ? null : ToFormViewModel(department);
    }

    public Task CreateAsync(DepartmentFormViewModel modelo)
    {
        var department = new Department
        {
            IsActive = true,
            UpdatedAt = null
        };

        ApplyChanges(modelo, department);
        
        return _repository.AddAsync(department);
    }

    public async Task<bool> UpdateAsync(DepartmentFormViewModel modelo)
    {
        var department = await _repository.GetByIdAsync(modelo.Id);

        if (department is null)
        {
            return false;
        }

        ApplyChanges(modelo, department);

        department.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(department);
        
        return true;
    }

    public Task<bool> DeactivateAsync(Guid id)
    {
        return ChangeStatusAsync(id, true);
    }

    public Task<bool> ActivateAsync(Guid id)
    {
        return ChangeStatusAsync(id, true);
    }

    public Task<bool> CodeExistsAsync(string code, Guid? excluirId = null)
    {
        return _repository.CodeExistsAsync(NormalizeCode(code), excluirId);
    }

    public Task<bool> NameExistsAsync(string name, Guid? excluirId = null)
    {
        return _repository.NameExistsAsync(name.Trim(), excluirId);
    }

    private async Task<bool> ChangeStatusAsync(Guid id, bool activo)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department is null)
        {
            return false;
        }

        department.IsActive = activo;
        department.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(department);
        
        return true;
    }

    private static void ApplyChanges(DepartmentFormViewModel modelo, Department department)
    {
        department.Code = NormalizeCode(modelo.Code);

        department.Name = modelo.Name.Trim();
        
        department.Description = CleanOptional(modelo.Description);
        
        department.Location = CleanOptional(modelo.Location);
        
        department.Budget = modelo.Budget;
        
        department.Phone = CleanOptional(modelo.Phone);
        
        department.Email = CleanOptional(modelo.Email);
    }

    private static DepartmentListItemViewModel ToListItemViewModel(Department department)
    {
        return new DepartmentListItemViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            IsActive = department.IsActive
        };
    }

    private static DepartmentDetailsViewModel ToDetailsViewModel(Department department)
    {
        return new DepartmentDetailsViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            Phone = department.Phone,
            Email = department.Email,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }

    private static DepartmentFormViewModel ToFormViewModel(Department department)
    {
        return new DepartmentFormViewModel
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            Description = department.Description,
            Location = department.Location,
            Budget = department.Budget,
            Phone = department.Phone,
            Email = department.Email
        };
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }

    private static string? CleanOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}