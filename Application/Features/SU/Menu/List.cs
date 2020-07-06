using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Menu
{
    public class List
    {
        public class MenuVm
        {
            public string MenuCode { get; set; }
            public string Title { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public bool Active { get; set; }
            public string MainMenu { get; set; }
            public List<MenuVm> SubMenus { get; set; }
        }
        public class Query : IRequest<IEnumerable<MenuVm>>
        {
        }
        public class Handler : IRequestHandler<Query, IEnumerable<MenuVm>>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<IEnumerable<MenuVm>> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select m.menu_code,concat('menu.',m.menu_code) as title,p.program_path as url,m.main_menu as \"mainMenu\",m.icon,false as active");
                sql.AppendLine("from su_menu m");
                sql.AppendLine("left join su_program p on p.program_code = m.program_code");
                sql.AppendLine("where m.system_code = 'edu'");
                sql.AppendLine("	and  exists(select 'x'	");
                sql.AppendLine("	             from su_menu_profile mp 	");
                sql.AppendLine("	             inner join su_user_profile p on p.profile_code  = mp.profile_code 	");
                sql.AppendLine("	             where mp.menu_code  = m.menu_code 	");
                sql.AppendLine("	             and p.user_id = @UserId ) 	");

                sql.AppendLine("order by m.menu_code");
                var menus = await _context.QueryAsync<MenuVm>(sql.ToString(),new { 
                  UserId = _user.UserId
                },cancellationToken);
                var root = new MenuVm();
                this.GetMenus(root,menus, null);
                return root.SubMenus;
            }

            public void GetMenus(MenuVm parent,IEnumerable<MenuVm> menus,string menuCode)
            {
                var childs = menus.Where(o => o.MainMenu == menuCode);
                if (parent.SubMenus == null) parent.SubMenus = new List<MenuVm>();
                parent.SubMenus.AddRange(childs);
                foreach (var child in parent.SubMenus)
                    this.GetMenus(child, menus, child.MenuCode);

            }
        }
    }
}
