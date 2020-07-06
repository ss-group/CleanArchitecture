using Application.Common.Constants;
using Application.Common.Models;
using Domain.Entities.DB;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IContentService
    {
        Task<SuContent> FindById(long id, CancellationToken token);
        Task<SuContent> Save(IFormFile file,string category, CancellationToken token);
        void Remove(string fileName);
    }
}
