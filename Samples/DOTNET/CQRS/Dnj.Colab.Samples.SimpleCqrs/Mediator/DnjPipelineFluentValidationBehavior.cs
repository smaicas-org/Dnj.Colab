using Dnj.Colab.Samples.SimpleCqrs.Mediator.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Dnj.Colab.Samples.SimpleCqrs.Mediator;

public class DnjPipelineFluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator> _validators;

    public DnjPipelineFluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    /// <exception cref="DnjPipelineValidationException">validationFailures.Any().</exception>
    /// <exception cref="OutOfMemoryException">
    ///     The length of the resulting string overflows the maximum allowed length (
    ///     <see cref="System.Int32.MaxValue" />).
    /// </exception>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="ArgumentNullException">
    /// </exception>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<ValidationFailure> validationFailures = _validators
            .Select(validator => validator.Validate(new ValidationContext<IRequest<TResponse>>(request)))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .ToList();

        if (!validationFailures.Any()) return next();

        string error = string.Join("\r\n", validationFailures);
        throw new DnjPipelineValidationException(error);
    }
}