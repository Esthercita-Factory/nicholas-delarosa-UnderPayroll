using UnderPayroll.Web.ViewModels.Departments;

namespace UnderPayroll.Web.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentListItemViewModel>> GetAllAsync(bool? activo = null, string? busqueda = null);
    
    Task<DepartmentDetailsViewModel?> GetDetailsAsync(Guid id);
    
    Task<DepartmentFormViewModel?> GetForEditAsync(Guid id);
    
    Task CreateAsync(DepartmentFormViewModel modelo);
    
    Task<bool> UpdateAsync(DepartmentFormViewModel modelo);
    
    Task<bool> DeleteAsync(Guid id);

    Task<bool> HasEmployeesAsync(Guid id);
    
    Task<bool> DeactivateAsync(Guid id);
    
    Task<bool> ActivateAsync(Guid id);
    
    Task<bool> CodeExistsAsync(string code, Guid? excluirId = null);
    
    Task<bool> NameExistsAsync(string name, Guid? excluirId = null);
}