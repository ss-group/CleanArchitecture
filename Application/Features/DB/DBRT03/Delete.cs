using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT03
{
    public class Delete
    {
        public class Command : ICommand
        {
            public long ProvinceId { get; set; }
            public string ProvinceCode { get; set; }
            public uint RowVersion { get; set; }

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var province = new DbProvince { ProvinceId = request.ProvinceId, RowVersion = request.RowVersion };

                    _context.Set<DbProvince>().Attach(province);
                _context.Set<DbProvince>().Remove(province);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

        }
    }
}
