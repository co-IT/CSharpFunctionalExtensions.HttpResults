namespace CSharpFunctionalExtensions.HttpResults;

/// <summary>
/// Defines default ProblemDetails title and type mappings for HTTP status codes
/// and therefore allows customization of the library's default mapping behavior.
/// </summary>
public static class ProblemDetailsMappingProvider
{
  private static readonly Dictionary<int, (string? Title, string? Type)> InitialDefaultMappings = new()
  {
    { 400, ("Bad Request", "https://tools.ietf.org/html/rfc9110#section-15.5.1") },
    { 401, ("Unauthorized", "https://tools.ietf.org/html/rfc9110#section-15.5.2") },
    { 403, ("Forbidden", "https://tools.ietf.org/html/rfc9110#section-15.5.4") },
    { 404, ("Not Found", "https://tools.ietf.org/html/rfc9110#section-15.5.5") },
    { 405, ("Method Not Allowed", "https://tools.ietf.org/html/rfc9110#section-15.5.6") },
    { 406, ("Not Acceptable", "https://tools.ietf.org/html/rfc9110#section-15.5.7") },
    { 408, ("Request Timeout", "https://tools.ietf.org/html/rfc9110#section-15.5.9") },
    { 409, ("Conflict", "https://tools.ietf.org/html/rfc9110#section-15.5.10") },
    { 412, ("Precondition Failed", "https://tools.ietf.org/html/rfc9110#section-15.5.13") },
    { 415, ("Unsupported Media Type", "https://tools.ietf.org/html/rfc9110#section-15.5.16") },
    { 422, ("Unprocessable Entity", "https://tools.ietf.org/html/rfc4918#section-11.2") },
    { 426, ("Upgrade Required", "https://tools.ietf.org/html/rfc9110#section-15.5.22") },
    { 500, ("An error occurred while processing your request.", "https://tools.ietf.org/html/rfc9110#section-15.6.1") },
    { 502, ("Bad Gateway", "https://tools.ietf.org/html/rfc9110#section-15.6.3") },
    { 503, ("Service Unavailable", "https://tools.ietf.org/html/rfc9110#section-15.6.4") },
    { 504, ("Gateway Timeout", "https://tools.ietf.org/html/rfc9110#section-15.6.5") },
  };

  /// <summary>
  /// Gets or sets the default mappings for HTTP status codes to ProblemDetails titles and types.
  /// </summary>
  /// <remarks>
  /// This dictionary defines the default title and type values used for each status code.
  /// Users can modify it directly or use helper methods such as <see cref="AddOrUpdateMapping"/>.
  /// </remarks>
  public static Dictionary<int, (string? Title, string? Type)> DefaultMappings { get; set; } = InitialDefaultMappings;

  /// <summary>
  /// Finds the ProblemDetails title and type mapping for a given HTTP status code.
  /// </summary>
  /// <param name="statusCode">The HTTP status code to look up</param>
  /// <returns>A tuple containing the title and type, or (null, null) if no mapping exists</returns>
  public static (string? Title, string? Type) FindMapping(int statusCode)
  {
    return DefaultMappings.TryGetValue(statusCode, out var mapping) ? mapping : (null, null);
  }

  /// <summary>
  /// Adds or updates a mapping for a specific HTTP status code
  /// </summary>
  /// <param name="statusCode">HTTP status code to map</param>
  /// <param name="title">ProblemDetails title</param>
  /// <param name="type">ProblemDetails type URI</param>
  public static void AddOrUpdateMapping(int statusCode, string? title, string? type)
  {
    DefaultMappings[statusCode] = (title, type);
  }

  /// <summary>
  /// Resets all mappings to their initial default values, discarding any custom changes.
  /// </summary>
  public static void ResetToDefaults()
  {
    DefaultMappings = InitialDefaultMappings;
  }
}
