﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modules.Shared.Integration.Domain;

namespace Shared.Features.Persistence.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
	private readonly IMediator _mediator = mediator;

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

		return base.SavingChanges(eventData, result);

	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		await DispatchDomainEvents(eventData.Context);

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public async Task DispatchDomainEvents(DbContext? context)
	{
		if (context == null) return;

		var entities = context.ChangeTracker
				.Entries<BaseEntity>()
				.Where(e => e.Entity.EntityEvents.Count != 0)
				.Select(e => e.Entity);

		var domainEvents = entities
				.SelectMany(e => e.EntityEvents)
				.ToList();

		entities.ToList().ForEach(e => e.ClearEntityEvents());

		foreach (var domainEvent in domainEvents)
			await _mediator.Publish(domainEvent);
	}
}