using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNose.Domain.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException() { }

    public CategoryNotFoundException(string? message) : base(message) { }
}
