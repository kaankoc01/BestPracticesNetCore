using App.Repositories.Extension;
using App.Services.Extensions;
using App.Services.Filters;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; // referans türler için null kontrolünü geçmicek
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(x => { });


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


//Repository , unitofWork(savechanges()) kullanmýyoruz.
// unitofwork.savechanges() stock , payment, order örneði