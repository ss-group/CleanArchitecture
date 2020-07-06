using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Features.Services;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Email;
using Domain.Entities.DB;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class Create
    {
        public class Result
        {
            public long Id { get; set; }
            public bool haveEmail { get; set; }
        }
        public class Command : SuUser, ICommand<Result>
        {
            public long? StudentId { get; set; }
            public string EmployeeCode { get; set; }
            public new string UserType { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            private readonly IIdentityService _identity;
            private readonly IEmailSender _email;
            private readonly ILogger<Handler> _logger;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user, IIdentityService identity, IEmailSender email, ILogger<Handler> logger)
            {
                _context = context;
                _user = user;
                _identity = identity;
                _email = email;
                _logger = logger;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _context.Set<SuUser>().AnyAsync(o => o.UserName.ToLower() == request.UserName.ToLower(), cancellationToken);

                if (exists)
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT06.Username");
                }

                var studentType = await _context.GetParameterValue<string>("SuUserType", "Student", cancellationToken);
                var saveResult = new Result();
                DbEmployee employee = null;

                string password = string.Empty;


                employee = await _context.Set<DbEmployee>().FirstOrDefaultAsync(o => o.CompanyCode == _user.Company && o.EmployeeCode == request.EmployeeCode, cancellationToken);
                password = employee.EmployeeCode;


                request.CreatedBy = _user.UserName;
                request.CreatedDate = DateTime.Now;
                request.CreatedProgram = _user.ProgramCode;
                request.UpdatedBy = _user.UserName;
                request.UpdatedDate = DateTime.Now;
                request.UpdatedProgram = _user.ProgramCode;
                var result = await _identity.CreateUserAsync(request, password);

                var userType = new SuUserType
                {
                    UserId = result.UserId,
                    CompanyCode = _user.Company,
                    UserType = request.UserType
                };
                if (request.UserType == studentType)
                {
                    userType.StudentId = request.StudentId;
                }
                else
                {
                    userType.EmployeeCode = request.EmployeeCode;
                }
                _context.Set<SuUserType>().Add(userType);
                await _context.SaveChangesAsync(cancellationToken);

                saveResult.haveEmail = false;
                if (request.UserType != studentType && employee != null && !string.IsNullOrWhiteSpace(employee?.Email))
                {
                    try
                    {
                        var param = new Dictionary<string, string>();
                        param.Add("[UserName]", request.UserName);
                        param.Add("[Password]", password);
                        await _email.SendEmailWithTemplateAsysnc("SU002", employee.Email, null, param);
                        saveResult.haveEmail = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "SURT06 send create user email fail.");
                    }
                }
                
                saveResult.Id = result.UserId;
                return saveResult;
            }
        }

    }
}
