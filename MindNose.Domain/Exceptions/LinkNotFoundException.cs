using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNose.Domain.Exceptions;

public class LinkNotFoundException : Exception
{
    public LinkNotFoundException() { }

    public LinkNotFoundException(string? message) : base(message) { }
}
