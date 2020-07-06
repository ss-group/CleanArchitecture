using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT15
{
    public class Detail
    {
        public class DbDegreeSubVm : DbDegreeSub
        {
            public IEnumerable<DbDegreeSubEduGroup> DbDegreeSubEduGroup { get; set; }
        }

        public class DbEduSubDegreeGroupVm : DbDegreeSubEduGroup
        {
        }
        public class Query : IRequest<DbDegreeSub>
        {
            public long SubDegreeId { get; set; }
        }
        public class Handler : IRequestHandler<Query, DbDegreeSub>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbDegreeSub> Handle(Query request, CancellationToken cancellationToken)
                {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select spt.*,spt.xmin as \"rowVersion\" ");
                sql.AppendLine("from db_degree_sub as spt");
                sql.AppendLine("where spt.sub_degree_id = @SubDegreeId");

                var subj = await _context.QueryFirstOrDefaultAsync<DbDegreeSubVm>(sql.ToString(), new { SubDegreeId = request.SubDegreeId }, cancellationToken);

                StringBuilder sqlDegreeSub = new StringBuilder();
                sqlDegreeSub.AppendLine("select spp.*, spp.xmin as \"rowVersion\" ");
                sqlDegreeSub.AppendLine("from db_degree_sub_edu_group as spp ");
                sqlDegreeSub.AppendLine("where spp.sub_degree_id = @SubDegreeId");
                subj.DbDegreeSubEduGroup = await _context.QueryAsync<DbDegreeSubEduGroup>(sqlDegreeSub.ToString(), new { SubDegreeId = request.SubDegreeId }, cancellationToken);
                
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
