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

namespace Application.Features.DB.DBRT17
{
    public class Detail
    {
        public class DbBuildingVm : DbBuilding
        {
            public IEnumerable<DbRoom> DbRoom { get; set; }
            public IEnumerable<DbPrivilegeBuilding> DbPrivilegeBuilding { get; set; }
        }

        public class DbRoomVm : DbRoom
        {
        }
        public class DbPrivilegeBuildingVm : DbPrivilegeBuilding
        {
        }
        public class Query : IRequest<DbBuilding>
        {
            public long BuildingId { get; set; }
        }
        public class Handler : IRequestHandler<Query, DbBuilding>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbBuilding> Handle(Query request, CancellationToken cancellationToken)
                {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select b.*, b.xmin as \"rowVersion\" ");
                sql.AppendLine("from db_building b");
                sql.AppendLine("where b.building_id = @BuildingId");
                sql.AppendLine("AND b.company_code = @CompanyCode ");
                var subj = await _context.QueryFirstOrDefaultAsync<DbBuildingVm>(sql.ToString(), new { CompanyCode = _user.Company, BuildingId = request.BuildingId }, cancellationToken);

                StringBuilder sqlRoom = new StringBuilder();
                sqlRoom.AppendLine("select r.*, r.xmin as \"rowVersion\" ");
                sqlRoom.AppendLine("from db_room r ");
                sqlRoom.AppendLine("where r.building_id = @BuildingId");
                subj.DbRoom = await _context.QueryAsync<DbRoom>(sqlRoom.ToString(), new { BuildingId = request.BuildingId }, cancellationToken);

                StringBuilder sqlPrivilegeBuilding = new StringBuilder();
                sqlPrivilegeBuilding.AppendLine("select pb.*, pb.xmin as \"rowVersion\" ");
                sqlPrivilegeBuilding.AppendLine("from db_privilege_building pb ");
                sqlPrivilegeBuilding.AppendLine("where pb.building_id = @BuildingId");
                subj.DbPrivilegeBuilding = await _context.QueryAsync<DbPrivilegeBuilding>(sqlPrivilegeBuilding.ToString(), new { BuildingId = request.BuildingId }, cancellationToken);

                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
