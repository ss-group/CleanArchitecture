using Application.Common.Interfaces;
using Domain.Types;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long UserId
        {
            get => long.Parse(_httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        }
        public string UserName
        {
            get => _httpContextAccessor.HttpContext.User?.FindFirstValue(JwtClaimTypes.Name);
        }

        private StringValues _company;
        public string Company
        {
            get
            {
                _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("company", out _company);
                return _company;
            }
        }

        private StringValues _division;
        public string Division
        {
            get
            {
                _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("division", out _division);
                return _division;
            }
        }


        private StringValues _program;
        public string ProgramCode
        {
            get
            {
                _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("program", out _program);
                return _program;
            }
        }
        private StringValues _language;
        public string Language {
            get
            {
                _httpContextAccessor.HttpContext.Request?.Headers.TryGetValue("language", out _language);
                return _language;
            }
        }

        Lang _lang;
        public Lang Lang
        {
            get
            {
                Enum.TryParse(_language, out _lang);
                return _lang;
            }
        }
    }
}
