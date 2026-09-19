using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Data;
using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(
        bool soloActivos = false)
    {
        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .AsQueryable();

        if (soloActivos)
        {
            query = query.Where(e => e.IsActive);
        }

        return await query
            .OrderByDescending(e => e.IsActive)
            .ThenBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null)
        {
            return false;
        }

        _context.Employees.Remove(employee);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DocumentExistsAsync(
        string document,
        Guid? excluirId = null)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e =>
                e.Document == document &&
                (excluirId == null || e.Id != excluirId));
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        Guid? excluirId = null)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e =>
                e.Email == email &&
                (excluirId == null || e.Id != excluirId));
    }
}