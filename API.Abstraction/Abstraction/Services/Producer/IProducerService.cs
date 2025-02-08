using System;

namespace API.Application.Abstraction.Services.Producer;

public interface IProducerService
{
    Task ProduceAsync(string action, object message);
}
