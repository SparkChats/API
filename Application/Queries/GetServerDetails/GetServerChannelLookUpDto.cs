﻿using Application.Models;
using Application.Interfaces;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public class GetServerChannelLookUpDto : IMapWith<Channel>
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Channel, GetServerChannelLookUpDto>();
        }
    }
}