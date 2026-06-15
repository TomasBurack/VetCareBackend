using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;
using VetCareBackend.Application.Configuration;
using VetCareBackend.Application.Infrastructure;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Application.Services;
using VetCareBackend.Domain.Enums;
using VetCareBackend.Infrastructure;
using VetCareBackend.Infrastructure.ExternalService;
using VetCareBackend.Infrastructure.Repository;
using VetCareBackend.Presentation.Authorization;
using VetCareBackend.Presentation.Middlewares;

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

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresá el token JWT. No hace falta escribir 'Bearer', Swagger lo agrega solo."
    });

    opt.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", document), [] }
    });
});

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddDbContext<VetCareDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("VetCareConnectionStrings")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IVeterinarianRepository, VeterinarianRepository>();
builder.Services.AddScoped<IVeterinarianService, VeterinarianService>();
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<ISysadminRepository, SysadminRepository>();
builder.Services.AddScoped<ISysadminService, SysadminService>();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IMailService, MailService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.SoloClient, policy => policy.RequireRole(nameof(Role.Client), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.SoloVeterinarian, policy => policy.RequireRole(nameof(Role.Veterinarian), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.SoloAdministrator, policy => policy.RequireRole(nameof(Role.Administrator), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.SoloSysadmin, policy => policy.RequireRole(nameof(Role.SysAdmin), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.VetAdm, policy => policy.RequireRole(nameof(Role.Veterinarian), nameof(Role.Administrator), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.ClientAdm, policy => policy.RequireRole(nameof(Role.Client), nameof(Role.Administrator), nameof(Role.SysAdmin)));
    options.AddPolicy(Policies.Admins, policy => policy.RequireRole(nameof(Role.Administrator), nameof(Role.SysAdmin)));
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
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
