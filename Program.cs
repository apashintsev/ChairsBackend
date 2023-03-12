using ChairsBackend.DAL;
using ChairsBackend.Entities;
using ChairsBackend.HostedServices;
using ChairsBackend.Middlewares;
using ChairsBackend.Models;
using Gamebitcoin.WebAPI.SignalrHubs;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<GameSettings>(builder.Configuration.GetSection(nameof(GameSettings)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddDbContext<GameContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient
    );

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddHostedService<BotsService>();
builder.Services.AddScoped<IGameNotificationService, GameHub>();
builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (true/*app.Environment.IsDevelopment()*/)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/wss");
app.MapControllers();

Seed.EnsurePopulated(app);

app.Run();
