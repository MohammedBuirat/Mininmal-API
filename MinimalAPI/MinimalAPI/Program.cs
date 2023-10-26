using Microsoft.EntityFrameworkCore;
using MinimalAPI.DB.IRepository;
using MinimalAPI.DB.Repository;
using MinimalAPI.DB;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MinimalDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add controllers directly to the services collection
builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    });

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
