﻿using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequest : IRequest<PrivateChat>
    {
        public string ChatId { get; init; }
        public int MemberId { get; init; }
    }
}