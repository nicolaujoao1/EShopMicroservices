using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder();

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(options => { });  


app.Run();
