namespace UnderPayroll.Web.ViewModels.Departments;

public class DepartmentListItemViewModel
{
    public Guid Id { get; init; }
    
    public string Code { get; init; } = string.Empty;
    
    public string Name { get; init; } = string.Empty;
    
    public string? Description { get; init; }
    
    public string? Location { get; init; }
    
    public decimal? Budget { get; init; }
    
    public bool IsActive { get; init; }
}