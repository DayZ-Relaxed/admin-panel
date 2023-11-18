using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DayZRelaxed.Data;
using System.Globalization;
using DayZRelaxed.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
builder.Services.AddDbContext<DayZRelaxedContext0>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DayZRelaxedMap0")));
builder.Services.AddDbContext<DayZRelaxedContext1>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DayZRelaxedMap1")));

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    var cors = config.GetValue<string>("CorsUrl");
    builder.WithOrigins(cors).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

if (config.GetValue<string>("Environment").ToLower() == "production") builder.WebHost.UseIISIntegration();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

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
