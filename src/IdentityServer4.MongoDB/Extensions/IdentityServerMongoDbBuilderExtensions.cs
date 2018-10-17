using System;
using IdentityServer4.MongoDB.Options;
using IdentityServer4.MongoDB.Services;
using IdentityServer4.MongoDB.Stores;
using IdentityServer4.MongoDB.Tokens;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4.MongoDB.Extensions
{
    public static class IdentityServerMongoDbBuilderExtensions
    {
        /// <summary>
        /// Configures implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddConfigurationStore(
            this IIdentityServerBuilder builder,
            Action<StoreOptions> storeOptionsAction = null)
        {
            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddCorsPolicyService<CorsPolicyService>();

            var storeOptions = new StoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            builder.Services.AddIdentityServerMongoDbRepositories(storeOptions).AddIdentityServerMongoDbMappers();

            return builder;
        }

        /// <summary>
        /// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();
            builder.AddCorsPolicyCache<CorsPolicyService>();

            return builder;
        }

        /// <summary>
        /// Configures implementation of IPersistedGrantStore with IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            Action<StoreOptions> storeOptionsAction = null,
            Action<TokenCleanupOptions> tokenCleanUpOptionsAction = null)
        {
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

            var storeOptions = new StoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            builder.Services.AddIdentityServerMongoDbOperationalRepositories(storeOptions);

            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptionsAction?.Invoke(tokenCleanupOptions);
            builder.Services.AddSingleton(tokenCleanupOptions);

            return builder;
        }

        /// <summary>
        /// Adds an implementation of the IOperationalStoreNotification to IdentityServer.
        /// </summary>
        public static IIdentityServerBuilder AddOperationalStoreNotification<T>(
            this IIdentityServerBuilder builder)
            where T : class, IOperationalStoreNotification
        {
            builder.Services.AddTransient<IOperationalStoreNotification, T>();
            return builder;
        }
    }
}