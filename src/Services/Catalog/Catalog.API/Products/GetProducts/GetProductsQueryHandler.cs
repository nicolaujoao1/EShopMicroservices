namespace Catalog.API.Products.GetProducts;
public record GetProductsQuery():IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        var products= await session.Query<Product>().ToListAsync(cancellationToken); 

        return new GetProductResult(products);
    }
}
