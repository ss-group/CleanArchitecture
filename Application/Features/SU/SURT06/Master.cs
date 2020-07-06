using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;

namespace Application.Features.SU.SURT06
{
    public class Master
    {
        public class MasterData { }
        public class MasterSearchData : MasterData
        {
            public IEnumerable<dynamic> Statuses { get; set; }
            public IEnumerable<dynamic> UserTypes { get; set; }
        }

        public class MasterSaveData : MasterData
        {
            // public IEnumerable<dynamic> Employees { get; set; }
            public IEnumerable<dynamic> Policies { get; set; }
            public IEnumerable<dynamic> Profiles { get; set; }
        }
        public class MasterPermissionData : MasterData
        {
            public IEnumerable<dynamic> Companies { get; set; }

        }

        public class MasterDivisionData : MasterData
        {
            public IEnumerable<dynamic> Divisions { get; set; }
        }

        public class MasterEduLevelData: MasterData
        {
            public IEnumerable<dynamic> TypeLevels { get; set; }
        }
        public class Query : IRequest<MasterData>
        {
            public string Page { get; set; }
            public long? Id { get; set; }
            public string CompanyCode { get; set; }
            public string UserType { get; set; }
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
                var master = new MasterData() { };
                switch (request.Page)
                {
                    case "search":
                        master = new MasterSearchData
                        {
                            Statuses = await this.GetStatuses(cancellationToken),
                            UserTypes = await this.GetUserTypes(cancellationToken)
                        };
                        break;
                    case "detail":
                        master = new MasterSaveData
                        {
                            Policies = await this.GetPolicies(cancellationToken),
                            Profiles = await this.GetProfiles(cancellationToken)
                        };
                        break;
                    case "permission":
                        master = new MasterPermissionData
                        {
                            Companies = await this.GetCompanies(cancellationToken)
                        };
                        break;
                    case "division":
                        master = new MasterDivisionData
                        {
                            Divisions = await this.GetDivisions(request.CompanyCode, cancellationToken),
                        };
                        break;
                    case "eduLevel":
                        master = new MasterEduLevelData
                        {
                            TypeLevels = await this.GetTypeLevels(request.CompanyCode, cancellationToken)
                        };
                        break;
                }
                return master;
            }

            private Task<IEnumerable<dynamic>> GetStatuses(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT s.status_value as value,get_name(@Lang,status_desc_tha,status_desc_eng) as text,status_color as \"color\"");
                sql.AppendLine("FROM db_status s ");
                sql.AppendLine("WHERE s.table_name = 'su_user' ");
                sql.AppendLine("AND s.column_name = 'status' ");
                sql.AppendLine("ORDER BY s.sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetUserTypes(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT s.status_value as value,get_name(@Lang,status_desc_tha,status_desc_eng) as text");
                sql.AppendLine("FROM db_status s ");
                sql.AppendLine("WHERE s.table_name = 'su_user_type' ");
                sql.AppendLine("AND s.column_name = 'user_type' ");
                sql.AppendLine("ORDER BY s.sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);

            }

            private Task<IEnumerable<dynamic>> GetPolicies(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("	select password_policy_code as \"policyCode\",concat(password_policy_code,' : ',password_policy_name) as \"policyName\", password_age  as \"passwordAge\",active	");
                sql.AppendLine("	from su_password_policy	");
                sql.AppendLine("    order by password_policy_code ");
                return _context.QueryAsync<dynamic>(sql.ToString(), null, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetProfiles(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT s.profile_code as value,concat(s.profile_code,' : ',s.profile_desc) as text,s.active");
                sql.AppendLine("FROM su_profile s ");
                sql.AppendLine("ORDER BY text ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetCompanies(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT company_code as value,concat(company_code,' : ',get_name(@Lang,company_name_tha,company_name_eng)) as text,active");
                sql.AppendLine("FROM su_company ");
                sql.AppendLine("ORDER BY text ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetDivisions(string companyCode, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT company_code as \"companyCode\", div_code as \"divCode\",concat(div_code,' : ',get_name(@Lang,div_name_tha,div_name_eng)) as \"divName\",div_parent as \"divParent\"");
                sql.AppendLine("FROM su_division ");
                sql.AppendLine("where company_code = @Company ");
                sql.AppendLine("ORDER BY \"divName\" ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Company = companyCode, Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetTypeLevels(string companyCode,CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT company_code as \"companyCode\", education_type_level as value,concat(education_type_level,' : ',get_name(@Lang,education_type_level_name_tha,education_type_level_name_eng)) as text,active");
                sql.AppendLine("FROM db_education_type_level ");
                sql.AppendLine("where company_code = @Company ");
                sql.AppendLine("ORDER BY text ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Company = companyCode, Lang = _user.Language }, cancellationToken);
            }
        }
    }
}
