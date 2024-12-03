### Add Shared Context Migration
```dotnet ef migrations add InitialCreate -c ApplicationDbContext -p ../../Shared/Shared.Features -o Persistence/Migrations```

### Add Module Context Migration (Blog module sample)
```dotnet ef migrations add InitialCreate -c BlogContext -p ../../Modules/Blog/Modules.Blog.Features -o Persistence/Migrations```

### Add Module OAuth Migration (OAuth module sample)
```dotnet ef migrations add InitialCreate -c OAuthContext -p ../../Modules/OAuth/Modules.OAuth.Features -o Persistence/Migrations```

### Running Migrations (In development only)
Migration can be automatically applied by a middleware at runtime by executing the request at /dev-migration

### Generating mirgration scripts
dotnet ef migrations script -i -c BlogContext -p ../../Modules/Blog/Modules.Blog.Features -o ./Migrations/BlogContext.sql
dotnet ef migrations script -i -c UserContext -p ../../Modules/User/Modules.User.Features -o ./Migrations/UserContext.sql