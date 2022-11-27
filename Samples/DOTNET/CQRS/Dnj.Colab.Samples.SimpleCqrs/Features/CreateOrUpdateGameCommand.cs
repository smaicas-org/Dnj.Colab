using Dnj.Colab.Samples.SimpleCqrs.Data;
using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Features;

/// <summary>
/// COMMAND
/// </summary>
public class CreateOrUpdateGameCommand : IRequest<GameDto>
{
    public GameDto Game { get; set; }
}

/// <summary>
/// HANDLER
/// </summary>
public class CreateOrUpdateGameCommandHandler : IRequestHandler<CreateOrUpdateGameCommand, GameDto>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public CreateOrUpdateGameCommandHandler(IDbContextFactory<AppDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
    ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    public async Task<GameDto> Handle(CreateOrUpdateGameCommand request, CancellationToken cancellationToken)
    {
        await using AppDbContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        GameEntity entity = new()
        {
            Id = request.Game.Id,
            Title = request.Game.Title,
            Genre = request.Game.Genre,
            Platform = request.Game.Platform,
            ReleaseDate = request.Game.ReleaseDate
        };
        if (Guid.Empty.Equals(entity.Id))
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<GameEntity> res = await context.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            entity = res.Entity;
        }
        else
        {
            entity = context.Games.Update(entity).Entity;
        }
        await context.SaveChangesAsync(cancellationToken);
        request.Game.Id = entity.Id;
        return request.Game;
    }
}

/// <summary>
/// VALIDATOR
/// </summary>
public class CreateOrUpdateGameCommandValidator : AbstractValidator<CreateOrUpdateGameCommand>
{
    public CreateOrUpdateGameCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(dto => dto.Game).NotNull().WithMessage("Game is null");

        RuleLevelCascadeMode = CascadeMode.Continue;

        RuleFor(dto => dto.Game.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(dto => dto.Game.Genre).NotEmpty().WithMessage("Genre is required");
        RuleFor(dto => dto.Game.Platform).NotEmpty().WithMessage("Platform is required");
        RuleFor(dto => dto.Game.ReleaseDate).GreaterThan(default(DateTime)).WithMessage("Release Date cannot be default");

    }
}
