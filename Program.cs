using Microsoft.OpenApi.Models;
using System.Reflection;

var MyOrigins = "_MyOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddCors(options => {
options.AddPolicy(name: MyOrigins,
  policy  =>
  {
    policy.WithOrigins("https://cvnuxt.vercel.app/", "https://dotrest.azurewebsites.net/", "http://localhost:3000");
  });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dotrest API",
        Description = "An ASP.NET Core Web API for giving a github user's projects as a json",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseSwagger(options => {
  options.RouteTemplate = "/api/{documentName}/swagger.json";
});
app.UseSwaggerUI(options =>{
  options.SwaggerEndpoint("/api/v1/swagger.json", "Dotrest v1");
  options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors(MyOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
