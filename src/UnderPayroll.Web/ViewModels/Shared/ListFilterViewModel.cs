namespace UnderPayroll.Web.ViewModels.Shared;

/// <summary>
/// Estado de los filtros de un listado: estado activo y texto de búsqueda.
/// </summary>
/// <param name="Activo">true solo activos, false solo inactivos, null todos.</param>
/// <param name="Busqueda">Texto que escribió el usuario, si lo hay.</param>
/// <param name="Placeholder">Ayuda que se muestra en el campo de búsqueda.</param>
public record ListFilterViewModel(bool? Activo, string? Busqueda, string Placeholder);
