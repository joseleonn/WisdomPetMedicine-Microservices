using Microsoft.EntityFrameworkCore;
using WisdomPetMedicine.Rescue.ApplicationServices;
using WisdomPetMedicine.Rescue.Domain.Repositories;
using WisdomPetMedicine.Rescue.Extensions;
using WisdomPetMedicine.Rescue.Infraestructure;
using WisdomPetMedicine.Rescue.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRescueRepository, RescueRepository>();
builder.Services.AddScoped<AdopterApplicationService>();
builder.Services.AddHostedService<PetFlaggedForAdoptionIntegrationEventHandler>();
builder.Services.AddDbContext<RescueDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Rescue"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.EnsureRescueDbIsCreated();

app.MapControllers();

app.Run();
