using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Services
{
    public class GenerateRunningService : IService
    {
        private readonly ICleanDbContext _context;
        public GenerateRunningService(ICleanDbContext context)
        {

            _context = context;
        }

        public async Task<string> GetRunning(CancellationToken cancellationToken, RunningTypeCode typeCode, params string[] parameters)
        {
            var type = await _context.Set<DbRunningType>().Where(r => r.Code == typeCode.ToString()).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            string parameterSQL = string.Empty;
            if (type != null)
            {
                for (int i = 1; i <= parameters.Length; i++)
                {
                    parameterSQL = parameterSQL + $" AND parameter{i} = {{{i - 1}}}";
                }

                var running = await _context.Set<DbRunningNo>().FromSql($"SELECT *,xmin FROM db_running_no WHERE running_type_id = {type.RunningTypeId} {parameterSQL} FOR UPDATE", parameters).FirstOrDefaultAsync();
                if (running != null)
                {
                    running.RunningNo = ++running.RunningNo;
                }
                else
                {
                    running = new DbRunningNo();
                    running.RunningNo = 1;
                    running.RunningTypeId = type.RunningTypeId;
                    running.RunningTypeCode = type.Code;

                    for (int i = 1; i <= parameters.Length; i++)
                    {
                        PropertyInfo property = running.GetType().GetProperty($"Parameter{i}");
                        property.SetValue(running, parameters[i - 1]);
                    }
                    _context.Set<DbRunningNo>().Add(running);
                }
                await _context.SaveChangesAsync(cancellationToken);
                string runningDigit = running.RunningNo.ToString().PadLeft(type.RunningNoDigit, '0');
                try
                {
                    return string.Format(type.RunningNoFormat, parameters).Replace("RUNNING", runningDigit);
                }
                catch (Exception)
                {
                    throw new RestException(System.Net.HttpStatusCode.InternalServerError, "Parameter does not match with format string.");
                }
            }
            throw new RestException(System.Net.HttpStatusCode.InternalServerError, "Running type not found.");
        }
    }
}
