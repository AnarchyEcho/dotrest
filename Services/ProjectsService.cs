using dotrest.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace dotrest.Services;

public static class ProjectsService {
  static Task<List<Projects>> Projects { get; }
  static ProjectsService() {
    Projects = ProcessRepositories();
  }

  public static Task<List<Projects>> Get() => Projects;

  private static readonly HttpClient client = new HttpClient();

  private static async Task<List<Projects>> ProcessRepositories() {
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
    var streamTask = client.GetStreamAsync($"https://api.github.com/users/AnarchyEcho/repos");
    var repositories = await JsonSerializer.DeserializeAsync<List<Projects>>(await streamTask);
    #pragma warning disable 8603
    return repositories;
    #pragma warning restore 8603
  }
}
