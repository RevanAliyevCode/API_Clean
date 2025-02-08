using System;

namespace API.Domain.Exceptions;

public class NotFoundException : Exception
{
    public List<string> Errors { get; set; } = new List<string>();

    public NotFoundException(string errorMessages)
    {
        Errors.Add(errorMessages);
    }
}
