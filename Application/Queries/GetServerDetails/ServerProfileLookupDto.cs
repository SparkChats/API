﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record ServerProfileLookupDto : IMapWith<ServerProfile>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int UserId { get; init; }
        public RoleDto? MainRole { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile,
                ServerProfileLookupDto>();
        }
    }
}