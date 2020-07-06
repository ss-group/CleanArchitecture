using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Common.Models;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using Domain.Entities.SU;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Content
{
    public class UploadImage
    {
        public class Command : IRequest<SuContent>
        {
            public IFormFile File { get; set; }
            public string Category { get; set; }
        }

        public class Handler : IRequestHandler<Command, SuContent>
        {
            private readonly IContentService _contentService;
            public Handler(Func<ContentType, IContentService> contentService)
            {
                _contentService = contentService(ContentType.Image);
            }

            public async Task<SuContent> Handle(Command request, CancellationToken cancellationToken)
            {
               return await this._contentService.Save(request.File, request.Category, cancellationToken);
            }

        }
    }
}
