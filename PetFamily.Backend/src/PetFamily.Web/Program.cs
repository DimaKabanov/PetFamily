using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure;
using PetFamily.Volunteers.Presentation;
using PetFamily.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilog();

builder.Services
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersPresentation()
    .AddVolunteersApplication()
    .AddSpeciesInfrastructure()
    .AddSpeciesPresentation()
    .AddSpeciesApplication();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
