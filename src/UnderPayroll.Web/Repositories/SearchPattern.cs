namespace UnderPayroll.Web.Repositories;

/// <summary>
/// Prepara el texto de búsqueda del usuario para usarlo con ILIKE.
/// </summary>
internal static class SearchPattern
{
    /// <summary>
    /// Devuelve un patrón "contiene" con los comodines del usuario escapados,
    /// de modo que un "%" o un "_" escritos se busquen como texto literal.
    /// </summary>
    public static string Contains(string texto)
    {
        var escapado = texto
            .Trim()
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");

        return $"%{escapado}%";
    }
}
