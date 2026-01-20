using MyApiBoilerPlate.API;
using MyApiBoilerPlate.Application;
using MyApiBoilerPlate.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Enable DI validation to detect captive dependencies and registration errors at startup
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = builder.Environment.IsDevelopment();
    options.ValidateOnBuild = builder.Environment.IsDevelopment();
});

builder.Services.AddApplication();
builder.Services.AddPresentation();
builder.Services.AddInfrastructure();
builder.Services.AddExceptionHandling();

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
    });

    app.UseReDoc(options =>
    {
        options.SpecUrl = "/openapi/v1.json";
    });

    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();