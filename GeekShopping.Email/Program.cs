using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Connection Database
var connection = builder.Configuration["MySqlConnection:MySqlConnetionString"];

builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(connection,
                                                       ServerVersion.AutoDetect(connection)));

// Dependency Injection
var builderInjection = new DbContextOptionsBuilder<MySqlContext>();
builderInjection.UseMySql(connection, ServerVersion.AutoDetect(connection));

builder.Services.AddSingleton(new EmailRepository(builderInjection.Options));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
