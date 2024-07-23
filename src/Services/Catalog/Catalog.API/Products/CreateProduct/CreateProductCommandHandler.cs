namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(c => c.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler
    (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        var result = await validator.ValidateAsync(command, cancellationToken);

        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();

        if (errors.Any())
        {
            throw new ValidationException(errors.FirstOrDefault());
        }

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);

        await session.SaveChangesAsync(cancellationToken);

        logger.LogInformation("UpdateProductCommandHandler.Handle called with {@command}", command);

        return new CreateProductResult(product.Id);
    }
}
