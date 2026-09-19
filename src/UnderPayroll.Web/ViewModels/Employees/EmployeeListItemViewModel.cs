namespace UnderPayroll.Web.ViewModels.Employees;

public class EmployeeListItemViewModel
{
    public Guid Id { get; init; }

    public string Document { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string Position { get; init; } = string.Empty;

    public decimal? Salary { get; init; }

    public DateOnly HireDate { get; init; }

    public string DepartmentName { get; init; } = string.Empty;

    public Guid DepartmentId { get; init; }

    public bool IsActive { get; init; }
}