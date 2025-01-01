using Microsoft.EntityFrameworkCore;
using MyWebAPI.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();

//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


builder.Services.AddDbContext<DrugDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnectionString")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allows requests from any origin
              .AllowAnyMethod()  // Allows any HTTP method (GET, POST, etc.)
              .AllowAnyHeader(); // Allows any headers
    });
});


var app = builder.Build();
// Use CORS middleware
app.UseCors("AllowAll");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
