using AuthService.Config;
using AuthService.Config.Middlewares;
using AuthService.Data;
using AuthService.Services.Abstracts;
using Consul;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AuthDb") ?? "Data Source=./LocalDb/inner-auth.db"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var consulClient = new ConsulClient(config =>
{
    config.Address = new Uri(builder.Configuration.GetValue<string>("Consul:Address") ??
                             throw new ArgumentNullException(null, "Consul Address not found in configuration"));
});


var agentServiceRegistration = new AgentServiceRegistration()
{
    ID = $"auth-service-{Guid.NewGuid()}",
    Name = "auth-service",
    Address = "auth-service",
    Port = 8080,
    Check = new AgentServiceCheck()
    {
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
        Interval = TimeSpan.FromSeconds(10),
        HTTP = "http://auth-service:8080/health"
    }
};

var app = builder.Build();

app.MapGet(("/health"), () => new { Message = "Healthy" });
app.Lifetime.ApplicationStarted.Register(() =>
{
    consulClient.Agent.ServiceRegister(agentServiceRegistration).Wait();
    Console.WriteLine("Service registered");
});
app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(agentServiceRegistration.ID).Wait();
    Console.WriteLine("Service deregistered");
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<AuthDbContext>();
    dbContext?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program{}