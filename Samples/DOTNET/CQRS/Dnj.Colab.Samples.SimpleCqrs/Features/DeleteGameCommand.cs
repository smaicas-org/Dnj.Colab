/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Dnj.Colab.Samples.SimpleCqrs.Data;
using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Dnj.Colab.Samples.SimpleCqrs.Features.Responses;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Features;

/// <summary>
///     COMMAND
/// </summary>
public class DeleteGameCommand : IRequest<GenericStateResponse>
{
    public GameDto Game { get; set; }
}

/// <summary>
///     HANDLER
/// </summary>
public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, GenericStateResponse>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public DeleteGameCommandHandler(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

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
            ReleaseDate = request.Game.ReleaseDate
        };

        context.Games.Remove(entity);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            return new GenericStateResponse
            {
                State = StateEnum.Ko,
                Message = ex.Message
            };
        }

        return new GenericStateResponse
        {
            State = StateEnum.Ok
        };
    }
}