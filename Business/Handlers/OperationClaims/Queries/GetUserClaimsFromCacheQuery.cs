﻿
using Business.BusinessAspects;
using Core.Utilities.Results;
using Core.Aspects.Autofac.Performance;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Business.Constants;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Caching;
using Core.CrossCuttingConcerns.Caching;
using Microsoft.AspNetCore.Http;

namespace Business.Handlers.OperationClaims.Queries
{
    [SecuredOperation]
    public class GetUserClaimsFromCacheQuery : IRequest<IDataResult<IEnumerable<string>>>
    {
        public class GetUserClaimsFromCacheQueryHandler : IRequestHandler<GetUserClaimsFromCacheQuery, IDataResult<IEnumerable<string>>>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMediator _mediator;
            private readonly ICacheManager _cacheManager;
            private readonly IHttpContextAccessor _contextAccessor;

            public GetUserClaimsFromCacheQueryHandler(IOperationClaimRepository operationClaimRepository, IMediator mediator, ICacheManager cacheManager, IHttpContextAccessor contextAccessor)
            {
                _operationClaimRepository = operationClaimRepository;
                _mediator = mediator;
                _cacheManager = cacheManager;
                _contextAccessor = contextAccessor;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<IEnumerable<string>>> Handle(GetUserClaimsFromCacheQuery request, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;

                if (userId == null) throw new SecurityException(Messages.AuthorizationsDenied);
                var oprClaims = _cacheManager.Get($"{CacheKeys.UserIdForClaim}={userId}") as IEnumerable<string>;

                return new SuccessDataResult<IEnumerable<string>>(oprClaims);
            }
        }
    }
}