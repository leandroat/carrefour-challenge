
using Microsoft.EntityFrameworkCore;
using TransactionService.Services;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
#endregion

#region Dependency Injection
builder.Services.AddSingleton(configuration);
builder.Services.AddScoped<IReleaseProvider, ReleaseService>();
#endregion

#region Entity Framework (DB)
builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetValue<string>("Database:ConnectionString")));
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(51799);
    options.ListenAnyIP(44334);
});
builder.WebHost.UseKestrel();

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
