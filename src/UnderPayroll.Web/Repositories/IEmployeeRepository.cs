using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(
        bool soloActivos = false);

    Task<Employee?> GetByIdAsync(Guid id);

    Task AddAsync(Employee employee);

    Task UpdateAsync(Employee employee);
    
    Task<bool> DeleteAsync(Guid id);

    Task<bool> DocumentExistsAsync(
        string document,
        Guid? excluirId = null);

    Task<bool> EmailExistsAsync(
        string email,
        Guid? excluirId = null);
}