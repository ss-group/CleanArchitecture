using Application.Common.Interfaces;
using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<SuMenu> Menus { get; set; }
        public DbSet<SuMenuLabel> MenuLabels { get; set; }
        public DbSet<SuMenuProfile> MenuProfiles { get; set; }
        public DbSet<SuParameter> Parameters { get; set; }
        public DbSet<SuProfile> Profiles { get; set; }
        public DbSet<SuProgram> Programs { get; set; }
        public DbSet<SuProgramLabel> ProgramLabels { get; set; }
        public DbSet<SuUser> Users { get; set; }
        public DbSet<SuCompany> Companies { get; set; }
        public DbSet<SuDivision> Divisions { get; set; }
        public DbSet<SuContent> Contents { get; set; }
        public DbSet<SuUserType> UserTypes { get; set; }
        public DbSet<SuUserPermission> UserPermissions { get; set; }
        public DbSet<SuUserProfile> UserProfiles { get; set; }
        public DbSet<SuPasswordPolicy> SuPasswordPolicies { get; set; }
    }
}
