/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Dnj.Colab.Samples.SimpleCqrs.Mediator.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Dnj.Colab.Samples.SimpleCqrs.Mediator;

public class DnjPipelineFluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator> _validators;

    public DnjPipelineFluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <exception cref="DnjPipelineValidationException">validationFailures.Any().</exception>
    /// <exception cref="OutOfMemoryException">
    ///     The length of the resulting string overflows the maximum allowed length (
    ///     <see cref="System.Int32.MaxValue" />).
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException">
    /// </exception>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        List<ValidationFailure> validationFailures = _validators
            .Select(validator => validator.Validate(new ValidationContext<IRequest<TResponse>>(request)))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .ToList();

        if (!validationFailures.Any()) return next();
        throw new DnjPipelineValidationException(validationFailures);
        string error = string.Join("\r\n", validationFailures);
        throw new DnjPipelineValidationException(error);
    }
}