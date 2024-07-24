using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validatorResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var faitures = validatorResults.Where(r => r.Errors.Any()).SelectMany(c => c.Errors)
            .ToList();

        if (faitures.Any())
            throw new ValidationException(faitures);

        return await next();
    }
}
