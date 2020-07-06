using Application.Common.Constants;
using Application.Common.Models;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.SU;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Content
{
    public class Find
    {
        public class Query : IRequest<SuContent>
        {
            public long Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuContent>
        {
            private readonly IContentService _contentService;
            public Handler(Func<ContentType, IContentService> contentService)
            {
                _contentService = contentService(ContentType.File);
            }

            public async Task<SuContent> Handle(Query request, CancellationToken cancellationToken)
            {
                return await this._contentService.FindById(request.Id, cancellationToken);
            }
        }
    }
}
