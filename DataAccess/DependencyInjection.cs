﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.DataAccess.Repositories;

namespace Sparkle.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<PostgresDbContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("Postgres")
                    ?? throw new Exception("Connection doesn't exist");

                options.UseNpgsql(connectionString);
            });

            string mongoConnection = configuration.GetConnectionString("MongoDB")!;

            string connectionString = mongoConnection.Split(';')[0];
            string dbName = mongoConnection.Split(";")[1];

            services.AddMongoDB<MongoDbContext>(connectionString, dbName);

            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //TODO: figure out how to register BaseMongoRepository as IRepository
            services.AddScoped(typeof(IRepository<,>), typeof(BaseSqlRepository<,>));

            //TODO: figure out how to register all repositories at once
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IServerProfileRepository, ServerProfileRepository>();
            services.AddScoped<IRelationshipRepository, RelationshipRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}
