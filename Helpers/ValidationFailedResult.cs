using System.Net;
using GetGo.Booking.Helpers.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UserService.Dtos;

namespace UserService.Helpers;

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(CreateResponse(modelState))
    {
        StatusCode = StatusCodes.Status422UnprocessableEntity;
    }

    private static ApiResponse CreateResponse(ModelStateDictionary modelState)
    {
        var result = new ValidationResultModel(modelState);
        return ApiResponse.Error((int)HttpStatusCode.UnprocessableEntity, result.Message, result.Errors);
    }
}