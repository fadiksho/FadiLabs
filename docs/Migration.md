### Add Shared Context Migration
dotnet ef migrations add InitialCreate -c ApplicationDbContext -p ../../Shared/Shared.Features -o Persistence/Migrations

### Add Modules Context Migration
### Running from Web.Server
dotnet ef migrations add InitialCreate -c BlogContext -p ../../Modules/Blog/Modules.Blog.Features -o Persistence/Migrations
dotnet ef migrations add InitialCreate -c UserContext -p ../../Modules/User/Modules.User.Features -o Persistence/Migrations

### Remove Migration from Web.Server
dotnet ef migrations remove  -c UserContext -p ../../Modules/User/Modules.User.Features

## Update database

### Running from root
dotnet ef database update -c BlogContext -p src/Web/Web.Server/Web.Server.csproj
### Running from Web.Server
dotnet ef database update -c UserContext

### Running Migrations (In development only)
Migration can be automatically applied by a middleware at runtime by executing the request at /dev-migration

### Generating mirgration scripts
dotnet ef migrations script -i -c BlogContext -p ../../Modules/Blog/Modules.Blog.Features -o ./Migrations/BlogContext.sql
dotnet ef migrations script -i -c UserContext -p ../../Modules/User/Modules.User.Features -o ./Migrations/UserContext.sql

### Generating bundle
dotnet ef migrations bundle -c BlogContext -p ../../Modules/Blog/Modules.Blog.Features -o ./Migrations