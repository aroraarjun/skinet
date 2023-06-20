// Below create an instance(object) of this web application builder with some pre configured defualts
// So it will configure Kestrel web server(resposible for running web application provided by donet framweork][p=])
using API.Errors;
using API.Extensions;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Now when our app statrts it reads from configuration like appSettings.Development.json and appSettings.json 
// appSetings.json is read in all env but appSe.Devel.json is read and given precedence in development modde

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwagger();
app.UseSwaggerUI();

//From below line now our api server know that beside serving http request it also needs to serve static content
app.UseStaticFiles();

app.UseAuthorization();

// through this API knows where to send HTTP request
// This piece of middleware register controller endpoints so API knows where to send 
app.MapControllers();

//through using our service will be be disposed once finished executing
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during Migration");
}

app.Run();
