using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
    conf.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRespository, BasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>(); 

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
