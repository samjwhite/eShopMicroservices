//Vertical Slice Architecture

using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts => { 
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();


//Add services to the container.

var app = builder.Build();

//Configure the HTTP request pipeline.
app.MapCarter();

//Configure global exception handling
app.UseExceptionHandler();


app.Run();
