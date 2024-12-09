using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using CustomerManagementApp.Models;
using CustomerManagementApp.Serialization;
using DotNetEnv;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");

if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
{
    throw new InvalidOperationException("MongoDB connection settings are not set in the environment variables.");
}

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
