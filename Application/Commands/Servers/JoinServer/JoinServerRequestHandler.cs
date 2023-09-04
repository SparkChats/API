﻿using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.Servers.JoinServer
{
    public class JoinServerRequestHandler : RequestHandlerBase, IRequestHandler<JoinServerRequest>
    {
        public JoinServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task Handle(JoinServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Invitation invitation = await Context.Invitations.FindAsync(request.InvitationId);
            if (invitation.ExpireTime < DateTime.Now)
            {
                await Context.Invitations.DeleteAsync(invitation);
                throw new NoPermissionsException("The invitation is expired");
            }
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            if (server.ServerProfiles.Any(sp => sp.User.Id == UserId))
                throw new NoPermissionsException("You already a server member");
            if (server.BannedUsers.Contains(UserId))
                throw new NoPermissionsException("You are banned from the server");
            User user = await Context.SqlUsers.FindAsync(UserId);
            server.ServerProfiles.Add(new ServerProfile
            {
                User = Mapper.Map<UserLookUp>(user),
                Roles = new List<Role>(),
                DisplayName = user.DisplayName
            });
            await Context.Servers.UpdateAsync(server);
        }
    }
}