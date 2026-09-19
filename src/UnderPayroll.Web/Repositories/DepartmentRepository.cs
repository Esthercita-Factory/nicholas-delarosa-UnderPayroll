using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Data;
using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _baseDeDatos;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _baseDeDatos = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync(bool soloActivos = false)
    {
        var consulta = _baseDeDatos.Departments
            .AsNoTracking()
            .AsQueryable();

        if (soloActivos)
        {
            consulta = consulta.Where(d => d.IsActive);
        }
        
        return await consulta
            .OrderByDescending(d => d.IsActive)
            .ThenBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _baseDeDatos.Departments
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        _baseDeDatos.Departments.Add(department);
        await _baseDeDatos.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _baseDeDatos.Departments.Update(department);
        await _baseDeDatos.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var department = await _baseDeDatos.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
        {
            return false;
        }

        _baseDeDatos.Departments.Remove(department);

        await _baseDeDatos.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CodeExistsAsync(string code, Guid? excluirId = null)
    {
        return await _baseDeDatos.Departments
            .AsNoTracking()
            .AnyAsync(d => d.Code == code && (excluirId == null || d.Id != excluirId));
    }

    public async Task<bool> NameExistsAsync(string name, Guid? excluirId = null)
    {
        return await _baseDeDatos.Departments
            .AsNoTracking()
            .AnyAsync(d => d.Name == name && (excluirId == null || d.Id != excluirId));
    }
}