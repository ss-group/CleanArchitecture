using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> PersonalTaxTypes { get; set; }
            public IEnumerable<dynamic> Countries { get; set; }
            public IEnumerable<dynamic> Provinces { get; set; }
            public IEnumerable<dynamic> Districts { get; set; }
            public IEnumerable<dynamic> SubDistricts { get; set; }
            public IEnumerable<dynamic> PostalCodes { get; set; }
            public IEnumerable<dynamic> DivParents { get; set; }
            public IEnumerable<dynamic> DivTypes { get; set; }
            public IEnumerable<dynamic> Locations { get; set; }
            public IEnumerable<dynamic> Faculties { get; set; }
            public IEnumerable<dynamic> Programs { get; set; }
            public IEnumerable<dynamic> FacultiesOfPrograms { get; set; }
        }

        public class Query : IRequest<MasterData>
        {
            public string FormName { get; set; }
            public int CountryId { get; set; }
            public int ProvinceId { get; set; }
            public int DistrictId { get; set; }
            public int SubDistrictId { get; set; }
            public string DivCode { get; set; }
            public string CompanyCode { get; set; }
            public string ProgramCode { get; set; }
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
                var master = new MasterData();
                StringBuilder sql; ;

                foreach (string formName in request.FormName.Split(','))
                {
                    switch (formName.Trim().ToLower())
                    {
                        case "personaltaxtype":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END AS text,");
                            sql.AppendLine("            status_value AS value");
                            sql.AppendLine("FROM        db_status");
                            sql.AppendLine("WHERE       table_name = @TableName");
                            sql.AppendLine("            AND column_name = @ColumnName");
                            sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                            sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                            master.PersonalTaxTypes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName = "su_company", ColumnName = "personal_tax_type" }, cancellationToken);
                            break;

                        case "country":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN country_name_tha ELSE COALESCE(country_name_eng, country_name_tha) END AS text,");
                            sql.AppendLine("            country_id AS value,");
                            sql.AppendLine("            active");
                            sql.AppendLine("FROM        db_country");
                            sql.AppendLine("ORDER BY    CASE @Language WHEN @Tha THEN country_name_tha ELSE COALESCE(country_name_eng, country_name_tha) END");
                            master.Countries = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th" }, cancellationToken);
                            break;

                        case "province":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN province_name_tha ELSE COALESCE(province_name_eng, province_name_tha) END AS text,");
                            sql.AppendLine("            province_id AS value,");
                            sql.AppendLine("            active");
                            sql.AppendLine("FROM        db_province");
                            sql.AppendLine("WHERE       country_id = @CountryId");
                            sql.AppendLine("ORDER BY    CASE @Language WHEN @Tha THEN province_name_tha ELSE COALESCE(province_name_eng, province_name_tha) END");
                            master.Provinces = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", CountryId = request.CountryId }, cancellationToken);
                            break;

                        case "district":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN district_name_tha ELSE COALESCE(district_name_eng, district_name_tha) END AS text,");
                            sql.AppendLine("            district_id AS value,");
                            sql.AppendLine("            active");
                            sql.AppendLine("FROM        db_district");
                            sql.AppendLine("WHERE       province_id = @ProvinceId");
                            sql.AppendLine("ORDER BY    CASE @Language WHEN @Tha THEN district_name_tha ELSE COALESCE(district_name_eng, district_name_tha) END");
                            master.Districts = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", ProvinceId = request.ProvinceId }, cancellationToken);
                            break;

                        case "subdistrict":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN sub_district_name_tha ELSE COALESCE(sub_district_name_eng, sub_district_name_tha) END AS text,");
                            sql.AppendLine("            sub_district_id AS value,");
                            sql.AppendLine("            active");
                            sql.AppendLine("FROM        db_sub_district");
                            sql.AppendLine("WHERE       district_id = @DistrictId");
                            sql.AppendLine("ORDER BY    CASE @Language WHEN @Tha THEN sub_district_name_tha ELSE COALESCE(sub_district_name_eng, sub_district_name_tha) END");
                            master.SubDistricts = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", DistrictId = request.DistrictId }, cancellationToken);
                            break;

                        case "postalcode":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      postal_code AS text,");
                            sql.AppendLine("            postal_code_id AS value,");
                            sql.AppendLine("            active");
                            sql.AppendLine("FROM        db_postal_code");
                            sql.AppendLine("WHERE       province_id = @ProvinceId");
                            sql.AppendLine("            AND district_id = @DistrictId");
                            sql.AppendLine("            AND sub_district_id = @SubDistrictId");
                            sql.AppendLine("ORDER BY    CASE @Language WHEN @Tha THEN postal_name_tha ELSE COALESCE(postal_name_eng, postal_name_tha) END");
                            master.PostalCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", ProvinceId = request.ProvinceId, DistrictId = request.DistrictId, SubDistrictId = request.SubDistrictId }, cancellationToken);
                            break;

                        case "divparent":
                            sql = new StringBuilder();
                            sql.AppendLine("SELECT      CONCAT(div_code, ' : ', CASE @Language WHEN @Tha THEN div_name_tha ELSE COALESCE(div_name_eng, div_name_tha) END) AS text,");
                            sql.AppendLine("            div_code AS value");
                            sql.AppendLine("FROM        su_division");
                            sql.AppendLine("WHERE       company_code = @CompanyCode");

                            if (!string.IsNullOrEmpty(request.DivCode))
                            {
                                sql.AppendLine("        AND div_code != @DivCode");
                                sql.AppendLine("        AND (div_parent != @DivCode OR div_parent ISNULL)");
                            }

                            sql.AppendLine("ORDER BY    div_code");
                            master.DivParents = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", DivCode = request.DivCode, CompanyCode = request.CompanyCode }, cancellationToken);
                            break;

                        case "divtype":
                            sql = new StringBuilder();
                            sql.AppendLine("select 		case @Language when 'th' then status_desc_tha else coalesce(status_desc_eng, status_desc_tha) end as text,");
                            sql.AppendLine("			status_value as value");
                            sql.AppendLine("from   		db_status");
                            sql.AppendLine("where  		table_name  = 'su_division'");
                            sql.AppendLine("and    		column_name = 'div_type'");
                            sql.AppendLine("order by	sequence");
                            master.DivTypes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language }, cancellationToken);
                            break;

                        case "location":
                            sql = new StringBuilder();
                            sql.AppendLine("select      concat(location_code, ' : ', case @Language when 'th' then location_name_tha else coalesce(location_name_eng, location_name_tha) end) as text,");
                            sql.AppendLine("		    location_code as value,");
                            sql.AppendLine("		    active");
                            sql.AppendLine("from 	    db_location");
                            sql.AppendLine("where	    company_code = @CompanyCode");
                            sql.AppendLine("order by    location_code");
                            master.Locations = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode }, cancellationToken);
                            break;

                        case "faculty":
                            sql = new StringBuilder();
                            sql.AppendLine("select	    concat(fac_code, ' : ', case @Language when 'th' then fac_name_tha else coalesce(fac_name_eng, fac_name_tha) end) as text,");
                            sql.AppendLine("		    fac_code as value,");
                            sql.AppendLine("		    active");
                            sql.AppendLine("from 	    db_fac");
                            sql.AppendLine("where	    company_code = @CompanyCode");
                            sql.AppendLine("order by    fac_code");
                            master.Faculties = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode }, cancellationToken);
                            break;

                        case "program":
                            sql = new StringBuilder();
                            sql.AppendLine("select	    concat(program_code, ' : ', case @Language when 'th' then program_name_tha else coalesce(program_name_eng, program_name_tha) end) as text,");
                            sql.AppendLine("		    program_code as value,");
                            sql.AppendLine("		    active");
                            sql.AppendLine("from 	    db_program");
                            sql.AppendLine("where	    company_code = @CompanyCode");
                            sql.AppendLine("order by    program_code");
                            master.Programs = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode }, cancellationToken);
                            break;

                        case "facultyofprogram":
                            sql = new StringBuilder();
                            sql.AppendLine("select distinct	concat(df.fac_code, ' : ', case @Language when 'th' then df.fac_name_tha else coalesce(df.fac_name_eng, df.fac_name_tha) end) as text,");
                            sql.AppendLine("				df.fac_code as value,");
                            sql.AppendLine("				df.active");
                            sql.AppendLine("from 			db_fac_program as dfp");
                            sql.AppendLine("inner join		db_fac as df");
                            sql.AppendLine("on				df.fac_code = dfp.fac_code");
                            sql.AppendLine("				and df.company_code = dfp.company_code");
                            sql.AppendLine("where			dfp.company_code = @CompanyCode");
                            sql.AppendLine("				and dfp.program_code = @Program");
                            sql.AppendLine("order by        df.fac_code");
                            master.FacultiesOfPrograms = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode, Program = request.ProgramCode }, cancellationToken);
                            break;
                    }
                }

                return master;
            }
        }
    }
}
