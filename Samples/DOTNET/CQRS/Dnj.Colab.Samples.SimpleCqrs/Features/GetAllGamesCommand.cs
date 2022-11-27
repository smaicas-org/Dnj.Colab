/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Dnj.Colab.Samples.SimpleCqrs.Data;
using Dnj.Colab.Samples.SimpleCqrs.Data.Entities;
using Dnj.Colab.Samples.SimpleCqrs.RCL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dnj.Colab.Samples.SimpleCqrs.Features;

/// <summary>
///     COMMAND
/// </summary>
public class GetAllGamesCommand : IRequest<List<GameDto>>
{
}

/// <summary>
///     HANDLER
/// </summary>
public class GetAllGamesCommandHandler : IRequestHandler<GetAllGamesCommand, List<GameDto>>
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public GetAllGamesCommandHandler(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<List<GameDto>> Handle(GetAllGamesCommand request, CancellationToken cancellationToken)
    {
        await using AppDbContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        List<GameEntity> entities = await context.Games.ToListAsync(cancellationToken);
        List<GameDto> dtos = new();
        foreach (GameEntity gameEntity in entities)
            dtos.Add(new GameDto
            {
                Id = gameEntity.Id,
                Title = gameEntity.Title,
                Genre = gameEntity.Genre,
                Platform = gameEntity.Platform,
                ReleaseDate = gameEntity.ReleaseDate
            });
        return dtos;
    }
}