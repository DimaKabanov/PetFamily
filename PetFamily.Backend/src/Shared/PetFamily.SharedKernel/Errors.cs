namespace PetFamily.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id is null ? "" : $"for id: {id}";
            return Error.NotFound("record.not.found", $"record not found {forId}".Trim());
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.required", $"{label} is required");
        }
        
        public static Error ValueTooLong(int maxLength, string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.too.long", $"{label} to long. Max length {maxLength}");
        }
        
        public static Error ValueStillUsing(Guid id, string? name = null)
        {
            var label = name ?? "value";
            return Error.Failure("value.still.using", $"{label} with id '{id}' still in use");
        }
        
        public static Error ValueAlreadyExisting(Guid id, string? name = null)
        {
            var label = name ?? "value";
            return Error.Failure("value.already.existing", $"{label} with id '{id}' already existing");
        }
    }
}