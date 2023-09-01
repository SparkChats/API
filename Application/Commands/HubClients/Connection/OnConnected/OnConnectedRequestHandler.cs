﻿using Application.Application;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.HubClients.Connection.OnConnected
{
    public class OnConnectedRequestHandler : HubRequestHandlerBase, IRequestHandler<OnConnectedRequest>
    {
        public OnConnectedRequestHandler(IHubContextProvider hubContextProvider, IAuthorizedUserProvider userProvider) : base(hubContextProvider, userProvider)
        {
        }

        public async Task Handle(OnConnectedRequest request, CancellationToken cancellationToken)
        {
            UserConnections? userConnections = await Context.UserConnections.FindOrDefaultAsync(UserId)!;
            if (userConnections == null)
            {
                userConnections = new UserConnections
                    { UserId = UserId, Connections = new HashSet<string>() { request.ConnectionId } };
                await Context.UserConnections.AddAsync(userConnections);
            }
            else
            {
                userConnections.Connections.Add(request.ConnectionId);
                await Context.UserConnections.UpdateAsync(userConnections);
            }
        }
    }
}