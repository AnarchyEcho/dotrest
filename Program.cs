using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net.Http.Headers;
using System.Text.Json;

namespace dotrest;
class Program {
  private static async Task<List<Models.Repository>> ProcessRepositories() {
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

    var streamTask = client.GetStreamAsync("https://api.github.com/users/KodeAndre/repos");
    var repositories = await JsonSerializer.DeserializeAsync<List<Models.Repository>>(await streamTask);

    return repositories;
  }
  private static readonly HttpClient client = new HttpClient();
  static async Task Main(string[] args) {
    var repositories = await ProcessRepositories();
    foreach (var repo in repositories) {
      Console.WriteLine(repo.Name);
      Console.WriteLine(repo.Description);
      Console.WriteLine(repo.GitHubHomeUrl);
      Console.WriteLine(repo.Homepage);
      Console.WriteLine();
    }

    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    builder.Services.AddControllers();
    builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
      policy  =>
      {
        policy.WithOrigins("https://cvnuxt.vercel.app/", "https://dotrest.azurewebsites.net/");
      });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Dotrest API",
            Description = "An ASP.NET Core Web API for managing ToDo items",
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

    app.UseCors(MyAllowSpecificOrigins);

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
  }
}