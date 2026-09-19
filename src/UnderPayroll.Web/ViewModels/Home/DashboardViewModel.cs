namespace UnderPayroll.Web.ViewModels.Home;

/// <summary>
/// Cifras resumen que se muestran en la portada.
/// </summary>
public class DashboardViewModel
{
    public int ActiveEmployees { get; init; }

    public int InactiveEmployees { get; init; }

    public int ActiveDepartments { get; init; }

    /// <summary>Suma de los salarios de los empleados activos.</summary>
    public decimal MonthlyPayroll { get; init; }

    /// <summary>Departamentos activos con su cantidad de empleados activos.</summary>
    public IReadOnlyList<DepartmentHeadcount> Headcount { get; init; } = [];
}

public record DepartmentHeadcount(string Name, int Employees);
