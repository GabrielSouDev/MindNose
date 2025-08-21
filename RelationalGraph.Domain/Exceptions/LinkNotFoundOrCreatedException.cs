using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalGraph.Domain.Exceptions;

public class LinkNotFoundOrCreatedException : Exception
{
    public LinkNotFoundOrCreatedException() { }

    public LinkNotFoundOrCreatedException(string? message) : base(message) { }
}
