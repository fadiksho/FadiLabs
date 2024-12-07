namespace Modules.Blog.Integration.Post;

[LabAuthorize(LabsPermissions.BlogOwner)]
public record DeletePostById(Guid Id) : IRequest<Result>;
