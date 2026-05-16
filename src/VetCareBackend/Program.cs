using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using VetCareBackend.Infrastructure;

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

builder.Services.AddDbContext<VetCareDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("VetCareConnectionStrings")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
