using Application.Common.Interfaces;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public DbSet<DbBank> Banks { get; set; }
        public DbSet<DbBankBranch> BankBranches { get; set; }
        public DbSet<DbRunningType> RunningTypes { get; set; }
        public DbSet<DbRunningNo> RunningNoes { get; set; }
        public DbSet<DbEducationTypeLevel> EducationTypeLevel { get; set; }
        public DbSet<DbFac> Fac { get; set; }
        public DbSet<DbEducationType> EducationTypeCode { get; set; }
        public DbSet<DbProgram> Program { get; set; }
        public DbSet<DbDegree> Degrees { get; set; }
        public DbSet<DbDegreeSub> SubDegree { get; set; }
        public DbSet<DbDegreeSubEduGroup> DegreeSubEduGroup { get; set; }
        public DbSet<DbProvince> Province { get; set; }
        public DbSet<DbCountry> Country { get; set; }
        public DbSet<DbInstitute> Institutes { get; set; }
        public DbSet<DbListItem> ListItem { get; set; }
        public DbSet<DbDistrict> Districts { get; set; }
        public DbSet<DbPostalCode> PostalCode { get; set; }
        public DbSet<DbPreName> Prenames { get; set; }
        public DbSet<DbSubDistrict> SubDistricts { get; set; }
        public DbSet<DbHoliday> Holiday { get; set; }
        public DbSet<DbEmployee> Employee { get; set; }
        public DbSet<DbMajor> Majors { get; set; }
        public DbSet<DbRegion> Region { get; set; }
        public DbSet<DbProject> Project { get; set; }
        public DbSet<DbLocation> Location { get; set; }
        public DbSet<DbBuilding> Building { get; set; }
        public DbSet<DbRoom> Room { get; set; }
        public DbSet<DbPrivilegeBuilding> PrivilegeBuilding { get; set; }
        public DbSet<DbDepartment> Department { get; set; }
        public DbSet<DbFacProgram> FacProgram { get; set; }
        public DbSet<DbFacProgramDetail> FacProgramDetail { get; set; }
        public DbSet<DbSubProgram> DbSubProgram { get; set; }
    }
}
