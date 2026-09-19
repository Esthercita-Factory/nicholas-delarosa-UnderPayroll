using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false);
    
    Task<Department?> GetByIdAsync(Guid id);
    
    Task AddAsync(Department department);
    
    Task UpdateAsync(Department department);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<bool> CodeExistsAsync(string code, Guid? excluirId = null);
    
    Task<bool> NameExistsAsync(string name, Guid? excluirId = null);
}