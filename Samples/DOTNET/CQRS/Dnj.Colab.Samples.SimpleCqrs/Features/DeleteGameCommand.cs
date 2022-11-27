using Dnj.Colab.Samples.SimpleCqrs.Data;
using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Dnj.Colab.Samples.SimpleCqrs.Features.Responses;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Features;

/// <summary>
/// COMMAND
/// </summary>
public class DeleteGameCommand : IRequest<GenericStateResponse>
{
    public GameDto Game { get; set; }
}

/// <summary>
/// HANDLER
/// </summary>
public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, GenericStateResponse>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public DeleteGameCommandHandler(IDbContextFactory<AppDbContext> dbContextFactory) => _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>

    public async Task<GenericStateResponse> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        await using AppDbContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        GameEntity entity = new()
        {
            Id = request.Game.Id,
            Title = request.Game.Title,
            Genre = request.Game.Genre,
            Platform = request.Game.Platform,
            ReleaseDate = request.Game.ReleaseDate,
        };

        context.Games.Remove(entity);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            return new GenericStateResponse()
            {
                State = StateEnum.Ko,
                Message = ex.Message
            };
        }

        return new GenericStateResponse()
        {
            State = StateEnum.Ok
        };
    }
}
