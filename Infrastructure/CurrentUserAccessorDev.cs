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
    public class CurrentUserAccessorDev : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessorDev(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long UserId
        {
            get => 2;

        }
        public string UserName
        {
            get => "admin";
        }

        public string Company
        {
            get
            {
                return "001";
            }
        }

        public string Division
        {
            get
            {
                return "00101";
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
        public string Language
        {
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
