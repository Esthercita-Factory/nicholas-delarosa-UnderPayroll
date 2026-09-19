using System.ComponentModel.DataAnnotations;

namespace UnderPayroll.Web.ViewModels.Employees;

public class EmployeeFormViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Documento")]
    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(
        15,
        ErrorMessage = "El documento no puede superar los {1} caracteres.")]
    public string Document { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(
        60,
        ErrorMessage = "El nombre no puede superar los {1} caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(
        60,
        ErrorMessage = "El apellido no puede superar los {1} caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [StringLength(
        120,
        ErrorMessage = "El correo no puede superar los {1} caracteres.")]
    [EmailAddress(
        ErrorMessage = "El correo no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    [StringLength(
        20,
        ErrorMessage = "El teléfono no puede superar los {1} caracteres.")]
    [Phone(
        ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string? Phone { get; set; }

    [Display(Name = "Cargo")]
    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [StringLength(
        80,
        ErrorMessage = "El cargo no puede superar los {1} caracteres.")]
    public string Position { get; set; } = string.Empty;

    [Display(Name = "Salario")]
    [Range(
        0.01,
        999999999999.99,
        ErrorMessage = "El salario debe ser mayor que cero.")]
    public decimal Salary { get; set; }

    [Display(Name = "Fecha de contratación")]
    [Required(ErrorMessage = "La fecha de contratación es obligatoria.")]
    public DateOnly? HireDate { get; set; }

    [Display(Name = "Departamento")]
    [Required(ErrorMessage = "El departamento es obligatorio.")]
    public Guid? DepartmentId { get; set; }
}