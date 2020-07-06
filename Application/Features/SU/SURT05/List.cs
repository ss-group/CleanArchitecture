using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT05
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string ParameterGroupCode { get; set; }
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
                sql.AppendLine("    SELECT  parameter_group_code AS \"parameterGroupCode\",");
                sql.AppendLine("            parameter_code AS \"parameterCode\",");
                sql.AppendLine("            parameter_value AS \"parameterValue\",");
                sql.AppendLine("            remark,");
                sql.AppendLine("            xmin AS \"rowVersion\"");
                sql.AppendLine("    FROM    su_parameter");

                if (!string.IsNullOrWhiteSpace(request.Keyword) || !string.IsNullOrEmpty(request.ParameterGroupCode))
                {
                    sql.AppendLine("WHERE");

                    if (!string.IsNullOrEmpty(request.ParameterGroupCode))
                    {
                        sql.AppendLine("    parameter_group_code = @ParameterGroupCode");

                        if (!string.IsNullOrWhiteSpace(request.Keyword))
                            sql.AppendLine("AND");
                    }

                    if (!string.IsNullOrWhiteSpace(request.Keyword))
                    {
                        sql.AppendLine("        CONCAT( parameter_code,");
                        sql.AppendLine("                parameter_value,");
                        sql.AppendLine("                remark)");
                        sql.AppendLine("        ILIKE CONCAT('%', @Keyword, '%')");
                    }
                }

                return await _context.GetPage(sql.ToString(), new { ParameterGroupCode = request.ParameterGroupCode, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
