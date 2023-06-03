// Below create an instance(object) of this web application builder with some pre configured defualts
// So it will configure Kestrel web server(resposible for running web application provided by donet framweork][p=])
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Now when our app statrts it reads from configuration like appSettings.Development.json and appSettings.json 
// appSetings.json is read in all env but appSe.Devel.json is read and given precedence in development modde

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefualtConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// through this API knows where to send HTTP request
// This piece of middleware register controller endpoints so API knows where to send 
app.MapControllers();

app.Run();
