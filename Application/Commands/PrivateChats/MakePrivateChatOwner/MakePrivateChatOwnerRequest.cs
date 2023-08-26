﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.MakePrivateChatOwner
{
    public class MakePrivateChatOwnerRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public ObjectId MemberId { get; init; }
    }
}