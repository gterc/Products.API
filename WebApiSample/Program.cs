using DataAccess.Repositories;
using DataAccess;
using MediatR;
using Services.Interfaces;
using Microsoft.OpenApi.Models;
using Services;
using DataAccess.Profiles;
using Products.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddDbContext<ProductsDbContext>(options =>
//{
//    options.UseInMemoryDatabase("Products");
//});
builder.Services.AddSingleton<ProductsContext>();
//builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductsRepository, SQLProductsRepository>();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(ProductsHandlers).Assembly);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products.API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Products API V1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
