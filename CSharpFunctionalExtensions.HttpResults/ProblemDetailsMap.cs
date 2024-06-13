namespace CSharpFunctionalExtensions.HttpResults;

public class ProblemDetailsMap
{
    public static (string Title, string Type) Find(int statusCode)
    {
        return statusCode switch
        {
            400 => ("Bad Request", "https://tools.ietf.org/html/rfc9110#section-15.5.1"),
            401 => ("Unauthorized", "https://tools.ietf.org/html/rfc9110#section-15.5.2"),
            403 => ("Forbidden", "https://tools.ietf.org/html/rfc9110#section-15.5.4"),
            404 => ("Not Found", "https://tools.ietf.org/html/rfc9110#section-15.5.5"),
            405 => ("Method Not Allowed", "https://tools.ietf.org/html/rfc9110#section-15.5.6"),
            406 => ("Not Acceptable", "https://tools.ietf.org/html/rfc9110#section-15.5.7"),
            408 => ("Request Timeout", "https://tools.ietf.org/html/rfc9110#section-15.5.9"),
            409 => ("Conflict", "https://tools.ietf.org/html/rfc9110#section-15.5.10"),
            412 => ("Precondition Failed", "https://tools.ietf.org/html/rfc9110#section-15.5.13"),
            415 => ("Unsupported Media Type", "https://tools.ietf.org/html/rfc9110#section-15.5.16"),
            422 => ("Unprocessable Entity", "https://tools.ietf.org/html/rfc4918#section-11.2"),
            426 => ("Upgrade Required", "https://tools.ietf.org/html/rfc9110#section-15.5.22"),
            500 => ("An error occurred while processing your request.", "https://tools.ietf.org/html/rfc9110#section-15.6.1"),
            502 => ("Bad Gateway", "https://tools.ietf.org/html/rfc9110#section-15.6.3"),
            503 => ("Service Unavailable", "https://tools.ietf.org/html/rfc9110#section-15.6.4"),
            504 => ("Gateway Timeout", "https://tools.ietf.org/html/rfc9110#section-15.6.5"),
            _ => (string.Empty, string.Empty)
        };
    }
}