﻿using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeDisplayNameCommand : IRequest<User>
    {
        public string? DisplayName { get; set; }
    }
}
