using System;
using FluentValidation.Results;

namespace API.Domain.Exceptions;

public class ValidationException : Exception
{
    public List<string> Errors { get; set; } = new List<string>();

    public ValidationException(string errorMessages)
    {
        Errors.Add(errorMessages);
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
        {
            Errors.Add(failure.ErrorMessage);
        }
    }

    public ValidationException(List<string> errors)
    {
        Errors = errors;
    }
}
