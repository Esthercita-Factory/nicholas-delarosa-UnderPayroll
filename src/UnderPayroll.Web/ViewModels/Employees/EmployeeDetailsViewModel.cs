using System.ComponentModel.DataAnnotations;

namespace UnderPayroll.Web.ViewModels.Employees;

public class EmployeeDetailsViewModel
{
    public Guid Id { get; init; }

    [Display(Name = "Documento")]
    public string Document { get; init; } = string.Empty;

    [Display(Name = "Nombre")]
    public string FirstName { get; init; } = string.Empty;

    [Display(Name = "Apellido")]
    public string LastName { get; init; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    public string Email { get; init; } = string.Empty;

    [Display(Name = "Teléfono")]
    public string? Phone { get; init; }

    [Display(Name = "Cargo")]
    public string Position { get; init; } = string.Empty;

    [Display(Name = "Salario")]
    public decimal Salary { get; init; }

    [Display(Name = "Fecha de contratación")]
    public DateOnly HireDate { get; init; }

    [Display(Name = "Departamento")]
    public string DepartmentName { get; init; } = string.Empty;

    [Display(Name = "Estado")]
    public bool IsActive { get; init; }

    [Display(Name = "Creado")]
    public DateTimeOffset CreatedAt { get; init; }

    [Display(Name = "Última edición")]
    public DateTimeOffset? UpdatedAt { get; init; }

    public string FullName =>
        $"{FirstName} {LastName}";

    public string SalaryTexto =>
        Salary.ToString(
            "C2",
            new System.Globalization.CultureInfo("es-CO"));

    public string HireDateTexto =>
        HireDate.ToString("dd/MM/yyyy");

    public string CreatedAtTexto =>
        CreatedAt
            .ToLocalTime()
            .ToString(
                "g",
                new System.Globalization.CultureInfo("es-CO"));

    public string UpdatedAtTexto =>
        UpdatedAt.HasValue
            ? UpdatedAt.Value
                .ToLocalTime()
                .ToString(
                    "g",
                    new System.Globalization.CultureInfo("es-CO"))
            : "Sin ediciones";
}