var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Web_Server>("web-server");

builder.Build().Run();
