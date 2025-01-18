using System;

namespace Business.Wrappers;


public class Response
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
}
public class ResponseError : Response
{
    public List<string> Errors { get; set; } = new List<string>();

    public ResponseError()
    {
        Succeeded = false;
    }
}

public class ResponseSuccess<T> : Response
{
    public T Data { get; set; } = default!;

    public ResponseSuccess()
    {
        Succeeded = true;
    }
}
