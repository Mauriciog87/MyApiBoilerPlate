using Microsoft.AspNetCore.RateLimiting;
using MyApiBoilerPlate.API;
using MyApiBoilerPlate.Application;
using MyApiBoilerPlate.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Threading.RateLimiting;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Enable DI validation to detect captive dependencies and registration errors at startup
builder.Host.UseDefaultServiceProvider(options =>
{
  options.ValidateScopes = builder.Environment.IsDevelopment();
  options.ValidateOnBuild = builder.Environment.IsDevelopment();
});

builder.Services.AddApplication();
builder.Services.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandling();

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

// Health Checks - General liveness and SQL Server readiness
string connectionString = builder.Configuration.GetConnectionString("ConnectionString")!;
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "sqlserver", tags: ["ready"]);

// Rate Limiting - Fixed Window policy (configurable via appsettings.json)
builder.Services.AddRateLimiter(options =>
{
  options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
  options.AddFixedWindowLimiter("fixed", limiterOptions =>
  {
    limiterOptions.PermitLimit = builder.Configuration.GetValue("RateLimiting:PermitLimit", 100);
    limiterOptions.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:WindowSeconds", 60));
    limiterOptions.QueueLimit = builder.Configuration.GetValue("RateLimiting:QueueLimit", 0);
    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
  });
  options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
      RateLimitPartition.GetFixedWindowLimiter(
          partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
          factory: _ => new FixedWindowRateLimiterOptions
          {
            PermitLimit = builder.Configuration.GetValue("RateLimiting:PermitLimit", 100),
            Window = TimeSpan.FromSeconds(builder.Configuration.GetValue("RateLimiting:WindowSeconds", 60)),
            QueueLimit = builder.Configuration.GetValue("RateLimiting:QueueLimit", 0),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
          }));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();

  app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"));

  app.UseReDoc(options => options.SpecUrl = "/openapi/v1.json");

  app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

// Health Check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
  Predicate = _ => false // Liveness: always healthy if app is running
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
  Predicate = check => check.Tags.Contains("ready") // Readiness: includes SQL Server check
});

app.Run();