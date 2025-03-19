var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.OnlineShop_ApiService>("apiservice");

builder.AddProject<Projects.OnlineShop_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
