using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Data;
using UnderPayroll.Web.ViewModels.Home;

namespace UnderPayroll.Web.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> GetSummaryAsync()
    {
        var activeEmployees = await _context.Employees
            .CountAsync(e => e.IsActive);

        var inactiveEmployees = await _context.Employees
            .CountAsync(e => !e.IsActive);

        var activeDepartments = await _context.Departments
            .CountAsync(d => d.IsActive);

        var monthlyPayroll = await _context.Employees
            .Where(e => e.IsActive)
            .SumAsync(e => e.Salary);

        // Los cinco departamentos activos con más empleados activos
        // Se ordena sobre un tipo anónimo para que EF traduzca todo a SQL;
        // el record se crea después, ya en memoria
        var headcountRows = await _context.Departments
            .Where(d => d.IsActive)
            .Select(d => new
            {
                d.Name,
                Employees = _context.Employees.Count(e => e.DepartmentId == d.Id && e.IsActive)
            })
            .OrderByDescending(h => h.Employees)
            .ThenBy(h => h.Name)
            .Take(5)
            .ToListAsync();

        var headcount = headcountRows
            .Select(h => new DepartmentHeadcount(h.Name, h.Employees))
            .ToList();

        return new DashboardViewModel
        {
            ActiveEmployees = activeEmployees,
            InactiveEmployees = inactiveEmployees,
            ActiveDepartments = activeDepartments,
            MonthlyPayroll = monthlyPayroll,
            Headcount = headcount
        };
    }
}
