using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using CustomerManagementApp.Models;
using CustomerManagementApp.Serialization;
using MongoDB.Driver;
using CustomerManagementApp.Services.IServices;
using CustomerManagementApp.Services;
using CustomerManagementApp.Repositories.IRepositories;
using CustomerManagementApp.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoSettings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(mongoSettings.ConnectionString);  
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())   
{
    app.UseExceptionHandler("/Home/Error");
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
