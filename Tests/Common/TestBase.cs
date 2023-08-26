﻿using Application.Interfaces;
using Application.Mapping;
using Application.Providers;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tests.Common
{
    public class TestBase : IDisposable
    {
        private readonly Mock<IAuthorizedUserProvider> _userProvider;

        protected Ids Ids;
        protected IAppDbContext Context;
        protected readonly IMapper Mapper;
        protected IAuthorizedUserProvider UserProvider => _userProvider.Object;
        protected readonly CancellationToken CancellationToken = CancellationToken.None;

        public TestBase()
        {
            MapperConfiguration mapperConfig = new(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly)));
            Mapper = mapperConfig.CreateMapper();
            _userProvider = new();
        }

        protected void SetAuthorizedUserId(ObjectId id)
        {
            _userProvider.Setup(provider => provider.GetUserId()).Returns(id);
        }

        public void CreateDatabase()
        {
            Context = TestDbContextFactory.Create(out Ids);
            SetAuthorizedUserId(Ids.UserAId);
        }

        public void Dispose()
        {
            TestDbContextFactory.Destroy(Context);
        }
    }
}
