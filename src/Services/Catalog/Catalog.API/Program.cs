var builder = WebApplication.CreateBuilder();

builder.Services.AddCarter();
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

app.MapCarter();

app.Run();
