﻿namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryReponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));

            var response = result.Adapt<GetProductByCategoryReponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductByCategor")
        .Produces<GetProductByCategoryReponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}
