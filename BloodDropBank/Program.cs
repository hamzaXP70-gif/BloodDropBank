using BloodDropBank.Data;
using BloodDropBank.Repositories;
using BloodDropBank.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BloodDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BloodConn")));

// Register Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// Register Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHospitalService, HospitalService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BloodDropBank API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
