namespace Modules.Blog.Integration.Post;

[LabAuthorize(Permissions.BlogOwner)]
public record DeletePostById(Guid Id) : IRequest<Result>;
