using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Services;
using VetCareBackend.Infrastructure;
using VetCareBackend.Infrastructure.ExternalService;
using VetCareBackend.Infrastructure.Repository;
using VetCareBackend.Presentation.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(
        name:"v1",
        new OpenApiInfo()
        {
            Title = "VetCareAPI",
            Version = "v1"
        });

    var xmlfile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlpath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    opt.IncludeXmlComments(
        xmlpath, 
        includeControllerXmlComments: true
        );
});

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IPetRepository, PetRepository>();

builder.Services.AddScoped<IPetService, PetService>();

builder.Services.AddDbContext<VetCareDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("VetCareConnectionStrings")));
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.soloClient, policy => policy.RequireClaim("Role", "Client"));
    options.AddPolicy(Policies.soloVeterinarian, policy => policy.RequireClaim("Role", "Veterinarian"));
    options.AddPolicy(Policies.soloAdministrator, policy => policy.RequireClaim("Role", "Administrator"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
