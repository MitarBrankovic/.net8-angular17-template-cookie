namespace TemplateBackend.Domain.Exceptions;

public class UnsupportedColourException : Exception
{
    // Example of a custom exception

    public UnsupportedColourException(string code)
        : base($"Colour \"{code}\" is unsupported.")
    {
    }
}
