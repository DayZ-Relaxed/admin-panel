using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DayZRelaxed.Data;
using System.Globalization;
using DayZRelaxed.middleware;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DayZRelaxedContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DayZRelaxed")));


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://localhost:3001").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("corsapp");

app.UseWhen(
    context =>
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/api/oauth")) return false;

        return true;
    },
    branch => branch.AuthMiddleware());

app.Run();
