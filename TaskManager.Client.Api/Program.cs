using Microsoft.Extensions.Options;
using TaskManager.Client.Api.Middleware;
using TaskManager.Client.Application.Abstractions;
using TaskManager.Client.Application.Features.Tasks.Queries.GetTasks;
using TaskManager.Client.Infrastructure.HttpClients;
using TaskManager.Client.Infrastructure.Options;
using Microsoft.OpenApi;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "توکن JWT را وارد کنید."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetTasksQuery).Assembly));

builder.Services.Configure<TaskApiOptions>(
    builder.Configuration.GetSection(TaskApiOptions.SectionName));


builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<ForwardAuthorizationHandler>();

builder.Services.AddHttpClient<ITaskApiClient, TaskApiClient>(
    (serviceProvider, httpClient) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<TaskApiOptions>>()
            .Value;

        httpClient.BaseAddress = new Uri(options.BaseUrl);
    })
    .AddHttpMessageHandler<ForwardAuthorizationHandler>();

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(
    (serviceProvider, httpClient) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<TaskApiOptions>>()
            .Value;

        httpClient.BaseAddress = new Uri(options.BaseUrl);
    });

var jwtSection = builder.Configuration.GetSection("Jwt");

var issuer = jwtSection["Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is missing.");

var audience = jwtSection["Audience"]
    ?? throw new InvalidOperationException("JWT Audience is missing.");

var secretKey = jwtSection["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();