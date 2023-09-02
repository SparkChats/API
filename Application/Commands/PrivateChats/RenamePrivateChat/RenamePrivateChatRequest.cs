﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
        
        [Required]
        [DefaultValue("NewTitle")]
        public string NewTitle { get; init; }
    }
}