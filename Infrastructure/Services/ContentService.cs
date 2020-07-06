using Application.Common.Constants;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{

    public class ContentService : IContentService
    {
        private readonly ICleanDbContext _context;
        private string _sharedPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\contents");
            }
        }

        private ContentType _type { get; set; }

        public ContentService(ICleanDbContext context)
        {
            _context = context;
            this._type = ContentType.File;
        }
        public ContentService(ICleanDbContext context, ContentType type)
        {
            _context = context;
            this._type = type;
        }

        public async Task<SuContent> FindById(long id, CancellationToken token)
        {
            return await _context.Set<SuContent>().Where(o => o.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
        }
        public void Remove(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var info = new DirectoryInfo(this._sharedPath);
                if (info.Exists)
                {
                    File.Delete(Path.Combine(this._sharedPath, fileName));
                }
            }
        }

        public async Task<SuContent> Save(IFormFile file, string category, CancellationToken token)
        {
            var path = Path.Combine(this._sharedPath, _type.ToString(), category);
            if (file.Length > 0)
            {
                var info = new DirectoryInfo(path);

                if (!info.Exists)
                {
                    info.Create();
                }
                var name = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
                var fullPath = Path.Combine(path, name);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return await this.Save(Path.Combine(_type.ToString(), category, name), file.FileName, token);
            }
            return null;
        }

        private async Task<SuContent> Save(string path, string name, CancellationToken token)
        {
            var content = new SuContent
            {
                Name = name,
                Path = path,
                Reference = false,
            };
            _context.Set<SuContent>().Add(content);
            await _context.SaveChangesAsync(token);

            return content;
        }
    }
}
