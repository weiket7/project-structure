using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GetGo.Booking.Helpers.Validation;

public class ValidationResultModel
{
    public string Message { get; }

    public Dictionary<string, string[]> Errors { get; }

    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Message = "ValidationError";
        Errors = modelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
    }
}