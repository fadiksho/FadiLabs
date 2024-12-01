using FluentValidation;

namespace Modules.Shared.Integration.Queries;

public abstract record PagedFilterQuery
{
	public virtual int PageNumber { get; set; } = 1;
	public virtual int PageSize { get; set; } = 10;
}

public class PagedFilterQueryValidator : AbstractValidator<PagedFilterQuery>
{
	public PagedFilterQueryValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThanOrEqualTo(1);

		RuleFor(x => x.PageSize)
			.GreaterThanOrEqualTo(3);
	}
}