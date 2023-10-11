﻿using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using static Sparkle.Application.Common.Constants.Constants;

namespace Sparkle.Application.Common.Factories
{
    public class RoleFactory : IRoleFactory
    {
        // это не финальные наборы клеймом, а наборы клеймом для тестов

        private readonly IRoleRepository _roleRepository;

        public RoleFactory(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        //TODO: Add more claims
        private readonly string[] _serverMemberDefaultClaims
            = { Claims.ChangeServerName };

        //TODO: Add more claims
        private readonly string[] _serverOwnerDefaultClaims =
        {
            Claims.ChangeServerName,
            Claims.ManageServer,
            Claims.ManageRoles,
            Claims.ManageChannels
        };
        //TODO: Add more claims

        private readonly string[] _personalChatMemberClaims =
        {
            Claims.ManageRoles,
            Claims.ManageServer,
            Claims.ManageMessages,
            Claims.ManageChannels
        };

        private readonly string[] _groupChatMemberClaims =
        {
            Claims.ManageRoles,
            Claims.ManageServer,
            Claims.ManageMessages,
            Claims.ManageChannels
        };

        private readonly string[] _groupChatOwnerClaims =
        {
            Claims.ManageMessages,
        };

        private static List<ServerClaim> CreateClaimsForRole(Role role, params string[] claims)
        {
            if (!claims.All(claim => Claims.GetClaims().Contains(claim)))
                throw new ArgumentException("Invalid claim(s) provided");

            List<ServerClaim> serverClaims = new();

            foreach (string claim in claims)
            {
                ServerClaim serverClaim = new()
                {
                    ClaimType = claim,
                    RoleId = role.Id
                };
                serverClaims.Add(serverClaim);
            }

            return serverClaims;
        }

        private readonly Role _groupChatOwnerRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatOwnerId,
            Name = Constants.Constants.Roles.GroupChatOwnerName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 1,
        };

        private readonly Role _groupChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.GroupChatMemberId,
            Name = Constants.Constants.Roles.GroupChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };

        private readonly Role _personalChatMemberRole = new()
        {
            Id = Constants.Constants.Roles.PrivateChatMemberId,
            Name = Constants.Constants.Roles.PrivateChatMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };

        private readonly Role _serverOwnerRole = new()
        {
            Id = Constants.Constants.Roles.ServerOwnerId,
            Name = Constants.Constants.Roles.ServerOwnerName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 100,
        };

        private readonly Role _serverMemberRole = new()
        {
            Id = Constants.Constants.Roles.ServerMemberId,
            Name = Constants.Constants.Roles.ServerMemberName,
            Color = Constants.Constants.Roles.DefaultColor,
            ServerId = null,
            Priority = 0,
        };
        public Role PersonalChatMemberRole => _personalChatMemberRole;
        public Role GroupChatMemberRole => _groupChatMemberRole;
        public Role ServerOwnerRole => _serverOwnerRole;
        public Role ServerMemberRole => _serverMemberRole;
        public Role GroupChatOwnerRole => _groupChatOwnerRole;

        public string[] GroupChatOwnerClaims => _groupChatOwnerClaims;
        public string[] GroupChatMemberClaims => _groupChatMemberClaims;
        public string[] PersonalChatMemberClaims => _personalChatMemberClaims;
        public string[] ServerOwnerDefaultClaims => _serverOwnerDefaultClaims;
        public string[] ServerMemberDefaultClaims => _serverMemberDefaultClaims;

        public List<Role> GetDefaultServerRoles()
            => new() { ServerOwnerRole, ServerMemberRole };

        public List<Role> GetGroupChatRoles()
            => new() { GroupChatOwnerRole, GroupChatMemberRole };

        public Task<Role> CreateServerRoleAsync(string name, string color, int priority, string[] claims, string serverId)
        {
            Role role = new()
            {
                Name = name,
                Color = color,
                Priority = priority,
                ServerId = serverId,
            };

            List<ServerClaim> identityClaims = CreateClaimsForRole(role, claims);

            return CreateServerRoleAsync(role, identityClaims);
        }

        public async Task<Role> CreateServerRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims)
        {
            if (role.ServerId is null)
            {
                throw new ArgumentNullException(nameof(role.ServerId), "Server id cant be null in server role");
            }

            await _roleRepository.AddAsync(role);
            await _roleRepository.AddClaimsToRoleAsync(role, claims);

            return role;
        }

        public Task<Role> CreateServerRoleAsync(Role role, string[] strings)
        {
            List<ServerClaim> claims = CreateClaimsForRole(role, strings);

            return CreateServerRoleAsync(role, claims);
        }
    }
}
