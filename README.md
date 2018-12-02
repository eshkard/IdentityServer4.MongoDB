# IdentityServer4.MongoDB

MongoDB persistence layer for IdentityServer4 based on the Official [EntityFramework](https://github.com/IdentityServer/IdentityServer4.EntityFramework) persistence layer.

[![eshkard_net MyGet Build Status](https://www.myget.org/BuildSource/Badge/eshkard_net?identifier=d92ea344-0af7-420a-895d-2892e704aa54)](https://www.myget.org/)

## Simple Usage
```c#
// using default connection: mongodb://localhost/identityserver
identityServerBuilder
    .AddConfigurationStore()
    .AddOperationalStore();
```

## Config database connection and collection prefix
```C#
const string connectionString = "mongodb://db.local.com/mydb";
identityServerBuilder
    .AddConfigurationStore(options =>
    {
        options.CollectionNamePrefix = "ids_";
        options.ConnectionString = connectionString;
    })
    .AddOperationalStore(options =>
    {
        options.CollectionNamePrefix = "ids_";
        options.ConnectionString = connectionString;
    });
```

