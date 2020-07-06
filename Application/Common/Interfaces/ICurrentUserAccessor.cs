using Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserAccessor
    {
        long UserId { get; }
        string UserName { get; }
        string Company { get; }
        string Division { get; }
        string Language { get; }
        Lang Lang { get; }
        string ProgramCode { get; }

    }
}
