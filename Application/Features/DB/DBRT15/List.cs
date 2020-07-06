using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT15
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
        }
        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ds.sub_degree_id AS \"subDegreeId\" ");
                sql.AppendLine("  ,get_name(@lang,d.degree_name_tha,d.degree_name_eng) AS \"degreeId\" ");
                sql.AppendLine("  ,ds.sub_degree_name_tha AS \"subDegreeNameTha\" ");
                sql.AppendLine("  ,ds.sub_degree_name_eng AS \"subDegreeNameEng\" ");
                sql.AppendLine("  ,ds.sub_degree_short_name_tha AS \"subDegreeShortNameTha\" ");
                sql.AppendLine("  ,ds.sub_degree_short_name_eng AS \"subDegreeShortNameEng\" ");
                sql.AppendLine("  ,ds.active as \"active\" ");
                sql.AppendLine("  ,ds.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_degree_sub ds");
                sql.AppendLine("JOIN db_degree d ON ds.degree_id = d.degree_id");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(sub_degree_name_tha,sub_degree_name_eng,sub_degree_short_name_tha,sub_degree_short_name_eng,get_name(@lang,d.degree_name_tha,d.degree_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword , Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
