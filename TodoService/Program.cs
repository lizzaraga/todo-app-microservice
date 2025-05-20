using System.Text;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoService.Config;
using TodoService.Config.Middlewares;
using TodoService.Data;
using TodoService.Services.Abstracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<ITodoService, TodoService.Services.TodoService>();
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoDb") ?? "Data Source=./LocalDb/inner-todo.db"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Secret") ?? throw new ArgumentNullException(
                    null, "Jwt Secret not found in configuration"))
                )
        };
    });
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
    ID = $"todo-service-{Guid.NewGuid()}",
    Name = "todo-service",
    Address = "todo-service",
    Port = 8080,
    Check = new AgentServiceCheck()
    {
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
        Interval = TimeSpan.FromSeconds(10),
        HTTP = "http://todo-service:8080/health"
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
    var dbContext = scope.ServiceProvider.GetService<TodoDbContext>();
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