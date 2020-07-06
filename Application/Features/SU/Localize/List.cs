using Application.Common.Constants;
using Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Localize
{
    public class List
    {
        public class Localize
        {
            public JObject Menu { get; set; }
            public JObject Message { get; set; }
            public JObject Label { get; set; }
        }

        public class LocalizeModule
        {
            public JObject Label { get; set; }
        }
        public class Query : IRequest<Localize>
        {
            public string Lang { get; set; }

            public string Module { get; set; }
        }

        public class Handler : IRequestHandler<Query, Localize>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<Localize> Handle(Query request, CancellationToken cancellationToken)
            {
                JObject messageJson = JObject.Parse("{}");
                JObject menuJson = JObject.Parse("{}");
                if (request.Module == "ALL")
                {
                    var messages = await _context.QuerySingleAsync<string>(@"select COALESCE(json_object_agg(message_code, message_desc),'{ }') as message from su_message where lang_code = @lang::lang", new { lang = request.Lang }, token: cancellationToken);
                    messageJson = JObject.Parse(messages);

                    var menus = await _context.QuerySingleAsync<string>(@"select COALESCE(json_object_agg(menu_code, menu_name),'{}') from su_menu_label where system_code = 'bbo' AND lang_code =  @lang::lang", new { lang = request.Lang }, token: cancellationToken);
                    menuJson = JObject.Parse(menus);
                }

                var labels = await _context.QuerySingleAsync<string>(@"select COALESCE(json_object_agg(concat(program_code,'.',field_name), label_name),'{}') from su_program_label where lang_code = @lang::lang AND module_code = @module ", new { lang = request.Lang,module=request.Module }, token: cancellationToken);
                var labelJson = JObject.Parse(labels);

                return new Localize() { Message = messageJson, Menu = menuJson, Label = labelJson };

            }
        }
    }
}
