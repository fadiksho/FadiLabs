var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Web_Server>("web-server");

builder.AddProject<Projects.Modules_Blog_Server>("modules-blog-server");

builder.Build().Run();
