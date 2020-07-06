using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT17
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Location { get; set; }
            public IEnumerable<dynamic> RoomType { get; set; }
            public IEnumerable<dynamic> PrivilegeBuilding { get; set; }
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
                var location = await this.GetLocation(cancellationToken);
                var roomType = await this.GetRoomType(cancellationToken);
                var privilegeBuilding = await this.GetPrivilegeBuilding(cancellationToken);

                return new MasterData { Location = location, RoomType = roomType, PrivilegeBuilding = privilegeBuilding };
            }

            private Task<IEnumerable<dynamic>> GetLocation(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(location_code,' : ',get_name(@lang,location_name_tha,location_name_eng)) as \"text\" ");
                sql.AppendLine(" ,location_code as \"value\" ");
                sql.AppendLine(" ,active as \"active\" ");
                sql.AppendLine(" FROM db_location ");
                sql.AppendLine(" WHERE company_code = @Company ");
                sql.AppendLine(" ORDER BY 1 ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Company = _user.Company, lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetRoomType(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT get_name(@lang,list_item_name_tha,list_item_name_eng) as \"text\" ");
                sql.AppendLine(" ,list_item_code as \"value\" ");
                sql.AppendLine(" ,active as \"active\" ");
                sql.AppendLine(" FROM db_list_item ");
                sql.AppendLine(" WHERE list_item_group_code = 'RoomType' ");
                sql.AppendLine(" ORDER BY sequence ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
            private Task<IEnumerable<dynamic>> GetPrivilegeBuilding(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(div_code,' : ',get_name(@lang,div_name_tha,div_name_eng)) as \"text\" ");
                sql.AppendLine(" ,div_code as \"value\" ");
                sql.AppendLine(" FROM su_division ");
                sql.AppendLine(" WHERE company_code = @Company ");
                sql.AppendLine(" ORDER BY div_code::numeric ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { Company = _user.Company, lang = _user.Language }, cancellationToken);
            }


        }
    }
}
    