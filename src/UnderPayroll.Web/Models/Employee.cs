using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnderPayroll.Web.Models;

public class Employee
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(15, ErrorMessage = "El documento no puede superar los 15 caracteres.")]
    [Column("document")]
    public string Document { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar los 60 caracteres.")]
    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(60, ErrorMessage = "El apellido no puede superar los 60 caracteres.")]
    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los 120 caracteres.")]
    [Column("email")]
    public string? Email  { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    [Column("phone")]
    public string? Phone  { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [StringLength(80, ErrorMessage = "El cargo no puede superar los 80 caracteres.")]
    [Column("position")]
    public string Position { get; set; } = string.Empty;
    
    [Column("salary", TypeName = "numeric(12, 2)")]
    public decimal? Salary { get; set; }
    
    [Required(ErrorMessage = "La fecha de contratación es obligatoria.")]
    [Column("hire_date")]
    public DateOnly HireDate { get; set; }
    
    [Required(ErrorMessage = "El departamento es obligatorio.")]
    [Column("department_id")]
    public Guid DepartmentId { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    // Relación con Department
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }
}