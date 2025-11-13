using Api.Extensions;
using Application;
using Asp.Versioning.Builder;
using Asp.Versioning;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHybridCache();
builder.AddRedisDistributedCache("redis");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApi();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

var routeGroupBuilder = app.MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);
app.MapEndpoints(routeGroupBuilder);

app.Run();
