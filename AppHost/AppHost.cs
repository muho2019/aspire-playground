var builder = DistributedApplication.CreateBuilder(args);

// Add database service
var sqlServer = builder.AddSqlServer("db").AddDatabase("appdata");

// Add redis cache service
var redis = builder.AddRedis("cache");

// Add API service and reference dependencies
var api = builder.AddProject<Projects.Api>("api")
    .WithReference(sqlServer)
    .WaitFor(sqlServer)
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();