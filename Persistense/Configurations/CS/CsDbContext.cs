﻿using Application.Common.Interfaces;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {

    }
}
