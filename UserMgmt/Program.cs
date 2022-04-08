using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using UserMgmt.Authorization;
using UserMgmt.Helpers;
using UserMgmt.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();

//Create In Memory DB
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Users"));
//Add Header to Swagger for Authorization
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//Dependency Injection
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IService, Service>();
var app = builder.Build();
//Add dummy data to DB
UserMgmt.DummyData.Initialize(app.Services.CreateScope().ServiceProvider);
// Configure the HTTP request pipeline. commented this out to deploy
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<JwtMiddleware>();
// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
