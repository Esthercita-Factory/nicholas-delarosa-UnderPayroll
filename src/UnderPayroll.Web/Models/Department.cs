using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnderPayroll.Web.Models;

public class Department
{
    public Guid Id { get; set; }
    
    public string Code { get; set; } = string.Empty;
    
    public string Name  { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public string? Location { get; set; } = string.Empty;
    
    public decimal? Budget { get; set; }
    
    public string? Phone  { get; set; } = string.Empty;
    
    public string? Email  { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}