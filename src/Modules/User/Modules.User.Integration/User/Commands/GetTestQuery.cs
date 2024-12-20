namespace Modules.User.Integration.User.Commands;

public record GetTestQuery : IRequest<Result>
{
}

[LabAuthorize]
public record GetTestAuthorizedQuery : IRequest<Result>
{
}