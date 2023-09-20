﻿using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Queries.RoleDetails
{
    public class RoleDetailsQueryHandler : RequestHandlerBase, IRequestHandler<RoleDetailsQuery, Role>
    {
        public RoleDetailsQueryHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(RoleDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(query.RoleId);
            return role;
        }
    }
}
