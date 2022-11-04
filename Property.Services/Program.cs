using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Property.Services.Data;
using Property.Services.Repositories;
using Property.Services.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = configuration["Identity:ApiName"];
        options.Authority = configuration["Identity:Url"];
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("weatherApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "read.write");
    });
});
builder.Services.AddControllers();
//builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
//builder.Services.AddScoped<IPropertiesRepository, PropertiesRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PropertyConnection")));

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
