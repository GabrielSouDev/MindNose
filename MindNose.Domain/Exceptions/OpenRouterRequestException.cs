using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNose.Domain.Exceptions;

public class OpenRouterRequestException : Exception
{
    public OpenRouterRequestException() { }

    public OpenRouterRequestException(string? message) : base(message) { }

    public OpenRouterRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
