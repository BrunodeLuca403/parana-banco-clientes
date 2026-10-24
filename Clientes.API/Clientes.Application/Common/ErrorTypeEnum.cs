using System;
using System.Collections.Generic;
using System.Text;

namespace Clientes.Application.Common
{
    public enum ErrorTypeEnum
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
    }
}
