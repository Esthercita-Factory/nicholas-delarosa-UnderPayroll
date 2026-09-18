using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnderPayroll.Web.Models;

// [Keyless]
[Table("departments")]
public class Department
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código no puede superar los 10 caracteres.")]
    [Column("code")]
    public string Code { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar los 60 caracteres.")]
    [Column("name")]
    public string Name  { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La localización es obligatoria.")]
    [StringLength(80, ErrorMessage = "La localización no puede superar los 80 caracteres.")]
    [Column("location")]
    public string Location { get; set; } = string.Empty;
    
    [Column("budget", TypeName = "numeric(14, 2)")]
    public decimal? Budget { get; set; }
    
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    [Column("phone")]
    public string? Phone  { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los 120 caracteres.")]
    [Column("email")]
    public string? Email  { get; set; } = string.Empty;
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}