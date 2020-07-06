using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT14
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> EducationTypeCode { get; set; }
            public IEnumerable<dynamic> FacCode { get; set; }
            public IEnumerable<dynamic> StudentTypeCode { get; set; }
        }

        public class Query : IRequest<MasterData>
        {

        }

        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                var educationTypeCode = await this.GetEducationTypeCode(cancellationToken);
                var fac = await this.Getfac(cancellationToken);
                var studentType = await this.GetstudentType(cancellationToken);
                return new MasterData { EducationTypeCode = educationTypeCode, FacCode = fac, StudentTypeCode = studentType }; 
            }

            private Task<IEnumerable<dynamic>> GetEducationTypeCode(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(education_type_code,' : ',get_name(@Lang,education_type_name_tha,education_type_name_eng)) AS \"text\" ");
                sql.AppendLine("  ,education_type_code AS \"value\" ");
                sql.AppendLine("  ,active AS \"active\" ");
                sql.AppendLine("FROM db_education_type");
                sql.AppendLine("WHERE company_code = @CompanyCode ");
                sql.AppendLine("ORDER BY education_type_code");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language, CompanyCode = _user.Company }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> Getfac(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(fac_code,' : ',get_name(@Lang,fac_name_tha,fac_name_eng)) AS \"text\" ");
                sql.AppendLine("  ,fac_code AS \"value\" ");
                sql.AppendLine("  ,active AS \"active\" ");
                sql.AppendLine("FROM db_fac");
                sql.AppendLine("WHERE company_code = @CompanyCode ");
                sql.AppendLine("ORDER BY 1");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { CompanyCode = _user.Company, User = _user.UserId, Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetstudentType(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(student_type_code,' : ',get_name(@Lang,student_type_name,student_type_name_eng)) AS \"text\" ");
                sql.AppendLine("  ,student_type_code AS \"value\" ");
                sql.AppendLine("  ,active AS \"active\" ");
                sql.AppendLine("FROM sh_student_type ");
                sql.AppendLine("ORDER BY 1");
                return _context.QueryAsync<dynamic>(sql.ToString(), new {Lang = _user.Language }, cancellationToken);
            }
        }
    }
}
