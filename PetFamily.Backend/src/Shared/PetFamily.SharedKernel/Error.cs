using PetFamily.SharedKernel.Enums;

namespace PetFamily.SharedKernel;

public record Error
{
    public const string SEPARATOR = "|";
    
    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public string? InvalidField { get; } = null;

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);
    
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);
    
    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }
    
    public static Error Deserialize(string serialized)
    {
        const int MIN_ERROR_PARTS_COUNT = 3;
        
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < MIN_ERROR_PARTS_COUNT)
            throw new ArgumentException("Invalid format");   

        if (!Enum.TryParse<ErrorType>(parts[2], out var type))
            throw new ArgumentException("Invalid format");

        return new Error(parts[0], parts[1], type);
    }

    public ErrorList ToErrorList() => new([this]);
}