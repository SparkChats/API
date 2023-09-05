﻿using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.GroupChats.MakeGroupChatOwner
{
    public class MakeGroupChatOwnerRequestHandler : RequestHandlerBase, IRequestHandler<MakeGroupChatOwnerRequest>
    {
        public async Task Handle(MakeGroupChatOwnerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(request.ChatId);
            if (pchat is not GroupChat chat) throw new Exception("This is not group chat");

            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("User is not an owner of the chat");
            if (!chat.Users.Any(u => u == request.MemberId))
                throw new NoSuchUserException("User in not a member of the chat");

            chat.OwnerId = request.MemberId;
            await Context.GroupChats.UpdateAsync(chat);
        }

        public MakeGroupChatOwnerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}