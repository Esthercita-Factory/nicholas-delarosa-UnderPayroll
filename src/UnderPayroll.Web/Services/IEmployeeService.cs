using UnderPayroll.Web.ViewModels.Employees;

namespace UnderPayroll.Web.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeListItemViewModel>> GetAllAsync(
        bool? activo = null,
        string? busqueda = null);

    Task<EmployeeDetailsViewModel?> GetDetailsAsync(Guid id);

    Task<EmployeeFormViewModel?> GetForEditAsync(Guid id);

    Task CreateAsync(EmployeeFormViewModel modelo);

    Task<bool> UpdateAsync(EmployeeFormViewModel modelo);
    
    Task<bool> DeleteAsync(Guid id);

    Task<bool> DeactivateAsync(Guid id);

    Task<bool> ActivateAsync(Guid id);

    Task<bool> DocumentExistsAsync(
        string document,
        Guid? excluirId = null);

    Task<bool> EmailExistsAsync(
        string email,
        Guid? excluirId = null);
}