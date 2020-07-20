using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class Master
    {
        public class MasterData { }
        public class MasterSearchData : MasterData
        {
            public IEnumerable<dynamic> Divlist { get; set; }
            public IEnumerable<dynamic> GroupTypeCode { get; set; }
            public IEnumerable<dynamic> StatusWork { get; set; }
        }
        public class MasterDetailData : MasterData
        {
            public IEnumerable<dynamic> PreNames { get; set; }

            //tab Prosonallity
            public IEnumerable<dynamic> Genders { get; set; }
            public IEnumerable<dynamic> Races { get; set; }
            public IEnumerable<dynamic> Nationalitys { get; set; }
            public IEnumerable<dynamic> Religions { get; set; }
            public IEnumerable<dynamic> AddressProvinces { get; set; }

            //tab Worker
            public IEnumerable<dynamic> EmployeeTypes { get; set; }
            public IEnumerable<dynamic> Divlists { get; set; }
            public IEnumerable<dynamic> PositionLevels { get; set; }
            public IEnumerable<dynamic> Positionlist { get; set; }
            public IEnumerable<dynamic> GroupTypeCode { get; set; }
            public IEnumerable<dynamic> PositionDivisions { get; set; }
            public IEnumerable<dynamic> JobTypes { get; set; }
            public IEnumerable<dynamic> DivWork { get; set; }

        }

        public class Query : IRequest<MasterData>
        {
            public string Page { get; set; }
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
                            Divlist = await this.GetDiv(cancellationToken),
                            GroupTypeCode = await this.GetGroupTypeCode(cancellationToken),
                            StatusWork = await this.GetStatusWork(cancellationToken),
                        };
                        break;
                    case "detail":
                        master = new MasterDetailData
                        {
                            PreNames = await this.GetPrename(cancellationToken),

                            //tab Prosonallity
                            Genders = await this.GetGender(cancellationToken),
                            Races = await this.GetRace(cancellationToken),
                            Nationalitys = await this.GetNationality(cancellationToken),
                            Religions = await this.GetReligion(cancellationToken),
                            AddressProvinces = await this.GetProvince(cancellationToken),

                            //tab Worker
                            EmployeeTypes = await this.GetEmployeeType(cancellationToken),
                            Divlists = await this.GetDiv(cancellationToken),
                            PositionLevels = await this.GetPositionLevel(cancellationToken),
                            Positionlist = await this.GetPosition(cancellationToken),
                            GroupTypeCode = await this.GetGroupTypeCode(cancellationToken),
                            PositionDivisions = await this.GetPositionDivision(cancellationToken),
                            JobTypes = await this.GetJobType(cancellationToken),
                            DivWork = await this.GetDivWork(cancellationToken),
                        };
                        break;
                }
                return master;
            }

            private Task<IEnumerable<dynamic>> GetDiv(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT div_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(div_code,' : ',get_name(@Lang, div_name_tha, div_name_eng)) AS \"text\" ");
                sql.AppendLine(" FROM su_division ");
                sql.AppendLine(" WHERE company_code = @Company ");
                sql.AppendLine(" ORDER BY length(div_code),div_code ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Company = _user.Company, Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetStatusWork(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(status_value,' : ',get_name(@Lang, status_desc_tha, status_desc_eng)) AS \"text\" ");
                sql.AppendLine(" ,status_value AS \"value\" ");
                sql.AppendLine(" FROM db_status ");
                sql.AppendLine(" WHERE table_name ='db_emp_status_work' ");
                sql.AppendLine(" AND column_name ='status_work' ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetPosition(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(position_id,' : ',get_name(@Lang,position_name,e_position_name)) AS \"text\" ");
                sql.AppendLine(" ,position_id AS \"value\" ");
                sql.AppendLine(" FROM erp_gb_position ");
                sql.AppendLine(" ORDER BY length(position_id),position_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetPrename(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT pre_name_id AS \"value\" ");
                sql.AppendLine(" ,get_name(@lang, pre_name_tha, pre_name_eng) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_pre_name ");
                sql.AppendLine(" ORDER BY pre_name_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetGender(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT list_item_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(list_item_code,' : ',get_name(@lang, list_item_name_tha, list_item_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = @ListItemGroupCode ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { ListItemGroupCode = "Gender", Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetRace(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT list_item_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(list_item_code,' : ',get_name(@lang, list_item_name_tha, list_item_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = @ListItemGroupCode ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { ListItemGroupCode = "Race", Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetNationality(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT list_item_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(list_item_code,' : ',get_name(@lang, list_item_name_tha, list_item_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = @ListItemGroupCode ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { ListItemGroupCode = "Nationality", Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetReligion(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT list_item_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(list_item_code,' : ',get_name(@lang, list_item_name_tha, list_item_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = @ListItemGroupCode ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { ListItemGroupCode = "Religion", Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetProvince(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(province_code,' : ',get_name('th', province_name_tha, province_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,province_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_province ");
                sql.AppendLine(" ORDER BY length(province_code),province_code ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetEmployeeType(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(emp_type_id,' : ',emp_type_name) AS \"text\" ");
                sql.AppendLine(" ,emp_type_id AS \"value\" ");
                sql.AppendLine(" FROM erp_gb_employee_type ");
                sql.AppendLine(" ORDER BY length(emp_type_id),emp_type_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetJobType(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(job_type,' : ',job_type_name) AS \"text\" ");
                sql.AppendLine(" ,job_type AS \"value\" ");
                sql.AppendLine(" FROM erp_pm_job_type ");
                sql.AppendLine(" ORDER BY length(job_type),job_type ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetPositionLevel(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(position_level_id,' : ',position_level_name) AS \"text\" ");
                sql.AppendLine(" ,position_level_id AS \"value\" ");
                sql.AppendLine(" FROM erp_gb_position_level ");
                sql.AppendLine(" ORDER BY length(position_level_id),position_level_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetGroupTypeCode(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT list_item_code AS \"value\" ");
                sql.AppendLine(" ,CONCAT(list_item_code,' : ',get_name(@lang,list_item_name_tha,list_item_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = @ListItemGroupCode ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { ListItemGroupCode = "GroupTypeCode", Lang = _user.Language }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetPositionDivision(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(position_id,' : ',get_name(@Lang,position_name,e_position_name)) AS \"text\" ");
                sql.AppendLine(" ,position_id AS \"value\" ");
                sql.AppendLine(" FROM erp_gb_position ");
                sql.AppendLine(" ORDER BY length(position_id),position_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetDivWork(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(division_id,' : ',get_name(@Lang,div_name,e_div_name)) AS \"text\" ");
                sql.AppendLine(" ,division_id AS \"value\" ");
                sql.AppendLine(" FROM erp_gb_division_work ");
                sql.AppendLine(" ORDER BY length(division_id),division_id ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Lang = _user.Language }, cancellationToken);
            }
        }
    }
}
