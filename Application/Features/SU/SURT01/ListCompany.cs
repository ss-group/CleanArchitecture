using Application.Common.Interfaces;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class ListCompany
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
                sql.AppendLine("SELECT      company_code AS \"companyCode\",");
                sql.AppendLine("            company_name_tha AS \"companyNameTha\",");
                sql.AppendLine("            company_name_eng AS \"companyNameEng\",");
                sql.AppendLine("            active,");
                sql.AppendLine("            xmin AS \"rowVersion\"");
                sql.AppendLine("FROM        su_company");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("WHERE   CONCAT(company_code,");
                    sql.AppendLine("               company_name_tha,");
                    sql.AppendLine("               company_name_eng)");
                    sql.AppendLine("        ILIKE CONCAT('%', @Keyword, '%')");
                }

                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
