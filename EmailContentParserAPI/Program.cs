using EmailContentParserAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Serilogging
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("Log/EmailContentParserLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
//Register connection context
builder.Services.AddDbContext<DbConnectionContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECPConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
