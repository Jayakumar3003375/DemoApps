using Microsoft.EntityFrameworkCore;
using Property.Minimal.Service.Data;
using Property.Minimal.Service.Models;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:PropertyConnection"]));
/// <summary>
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
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/properties", async (AppDbContext db) => await db.Properties.ToListAsync()).RequireAuthorization();

app.MapGet("/properties/{id}", async (int id, AppDbContext db) =>
    await db.Properties.Where(x => x.Id == id).FirstOrDefaultAsync() is Properties properties ? Results.Ok(properties) : Results.NotFound(new { statusCode = 404, statusMessage = "the Propertie id 'id' does not existing." })).RequireAuthorization();

app.MapPost("/properties", async (Properties properties, AppDbContext db) =>
{
    
        db.Properties.Add(properties);
        var affectedRowCount = await db.SaveChangesAsync();

        if (affectedRowCount > 0)
            return Results.Created($"/properties/{properties.Id}", properties);
        else
            return Results.StatusCode(500);
    
}).RequireAuthorization();
app.MapPut("/properties", async (Properties properties, AppDbContext db) =>
{
    var prop = await db.Properties.FirstOrDefaultAsync(x => x.Id == properties.Id);

    if (prop is null) return Results.NotFound();

    prop.PropertyNumber = properties.PropertyNumber;
    prop.Address = properties.Address;
    prop.City = properties.City;
    prop.Status = properties.Status;
    prop.Type = properties.Type;
    prop.Owner = properties.Owner;
    prop.CostPerSqft = properties.CostPerSqft;
    prop.NumberOfSqft = properties.NumberOfSqft;
    prop.TotalCost = properties.TotalCost;
    await db.SaveChangesAsync();

    return Results.Ok(prop);
}).RequireAuthorization(); 
app.MapDelete("/properties/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Properties.FindAsync(id) is Properties prop)
    {
        db.Properties.Remove(prop);
        await db.SaveChangesAsync();
        return Results.Ok(prop);
    }

    return Results.NotFound();
}).RequireAuthorization();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}