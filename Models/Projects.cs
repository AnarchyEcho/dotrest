using System.Text.Json.Serialization;

namespace dotrest.Models;
public class Projects {
  [JsonPropertyName("name")]
  public string? Name { get; set; }

  [JsonPropertyName("description")]
  public string? Description { get; set; }

  [JsonPropertyName("html_url")]
  public Uri? GitHubHomeUrl { get; set; }

  [JsonPropertyName("homepage")]
  public Uri? Homepage { get; set; }

  Random rnd = new Random();
  public int Id { get {return rnd.Next(999999);} }
}
