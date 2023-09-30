using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingTrackerAPI.Data;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(builder => 
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
    );
builder.Services.AddSwaggerGen(options =>
    {
       options.AddSecurityDefinition("oauth2",new OpenApiSecurityScheme
       {
           In = ParameterLocation.Header,
           Name= "Authorization",
           Type=SecuritySchemeType.ApiKey,
           
       }); 
       
       options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
    
    
    );
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // TODO validate issuer and audience based on appsettings.json
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SignKey").Value!))
    };
    options.SaveToken = true;
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();