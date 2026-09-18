namespace UnderPayroll.Web.Models;

public class Employee
{
    public Guid Id { get; set; }

    public string Document { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Position { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public DateOnly HireDate { get; set; }

    public Guid DepartmentId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public Department? Department { get; set; }
}