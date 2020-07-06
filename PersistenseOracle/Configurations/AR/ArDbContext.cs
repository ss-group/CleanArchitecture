using Application.Interfaces;
using Domain.Entities.AR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersistenseOracle
{
    public partial class OracleDbContext : DbContext, IOracleDbContext
    {
        public DbSet<ArGroup> ArGroups { get; set; }
    }
}
