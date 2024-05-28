using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Redis;
using RappidMQ.Entity;
using RappidMQ.IRepo;
using RappidMQ.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepoInterFace, Repo>();
builder.Services.AddDbContext<EntityDbContext>(option =>
{
    option.UseSqlite("Data Source=TestDatabase.db");
    
});
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = "127.0.0.1:6379"; });
//builder.Services.AddDistributedRedisCache(option =>
//{

//    option.Configuration = "127.0.0.1:6379,ssl=True,abortConnect=False";
//});
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

app.Run();
