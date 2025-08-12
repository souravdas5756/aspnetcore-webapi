using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Extensions;
using my_books.Services;
using Serilog;
//using Serilog.Events;
//using Serilog.Sinks.MSSqlServer;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


// Add services to the container.
builder.Host.UseSerilog(); // Use Serilog for logging
builder.Services.AddControllers();
// Register BooksService for dependency injection
builder.Services.AddScoped<BooksService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Use custom exception handler middleware
app.UseCustomExceptionHandler();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    AppDbInitializer.Initialize(dbContext);
}

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
