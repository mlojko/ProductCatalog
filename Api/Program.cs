using System.Text;
using System.Threading.RateLimiting;
using Api.Infrastructure.Helpers;
using Api.Infrastructure.Repositories;
using Api.Infrastructure.Services;
using Api.Models.Health;
using Api.Models.Products;
using Api.Models.Settings;
using Api.Models.Swagger;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings") ?? throw new InvalidOperationException("JwtSettings is not configured.");

#pragma warning disable CS8604 // Possible null reference argument.
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
#pragma warning restore CS8604 // Possible null reference argument.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
})
.AddMvc(options => { })
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Adding Rate Limiting
var rateLimitSttings = builder.Configuration.GetSection("RateLimits") ?? throw new InvalidOperationException("RateLimits is not configured.");


var permitLimit = int.Parse(rateLimitSttings["PermitLimit"] ?? "3");
var segmentsPerWindow = int.Parse(rateLimitSttings["SegmentsPerWindow"] ?? "5");
var queueLimit = int.Parse(rateLimitSttings["QueueLimit"] ?? "0");
var queueProcessingOrder = Enum.Parse<QueueProcessingOrder>(rateLimitSttings["QueueProcessingOrder"] ?? "OldestFirst");
var window = int.Parse(rateLimitSttings["Window"] ?? "10");

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                SegmentsPerWindow = segmentsPerWindow,
                QueueLimit = queueLimit,
                QueueProcessingOrder = queueProcessingOrder,
                Window = TimeSpan.FromSeconds(window)
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Register ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ProductService
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasherHelper, PasswordHasherHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddHealthChecks();
builder.Services.ConfigureHealthChecks(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(delegate (Options options)
{
    options.UIPath = "/healthcheck-ui";

});
app.UseRateLimiter();

app.Run();
public partial class Program { }