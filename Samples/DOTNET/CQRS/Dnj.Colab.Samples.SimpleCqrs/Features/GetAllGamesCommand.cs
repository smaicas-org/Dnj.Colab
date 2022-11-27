using Dnj.Colab.Samples.SimpleCqrs.Data;
using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Features;

/// <summary>
/// COMMAND
/// </summary>
public class GetAllGamesCommand : IRequest<List<GameDto>>
{
}

/// <summary>
/// HANDLER
/// </summary>
public class GetAllGamesCommandHandler : IRequestHandler<GetAllGamesCommand, List<GameDto>>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public GetAllGamesCommandHandler(IDbContextFactory<AppDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<List<GameDto>> Handle(GetAllGamesCommand request, CancellationToken cancellationToken)
    {
        await using AppDbContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        List<GameEntity> entities = await context.Games.ToListAsync(cancellationToken: cancellationToken);
        List<GameDto> dtos = new();
        foreach (GameEntity gameEntity in entities)
        {
            dtos.Add(new GameDto()
            {
                Id = gameEntity.Id,
                Title = gameEntity.Title,
                Genre = gameEntity.Genre,
                Platform = gameEntity.Platform,
                ReleaseDate = gameEntity.ReleaseDate
            });
        }
        return dtos;
    }
}
