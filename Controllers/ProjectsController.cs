using dotrest.Models;
using dotrest.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace dotrest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableCors("_MyOrigins")]
public class ProjectsController : ControllerBase {
  public ProjectsController() {
  }
  /// <summary>
  /// Fetches and displays github user's projects
  /// </summary>
  /// <response code="200">Returns the projects data</response>
  /// <response code="400">No data to return</response>
  [HttpGet(Name = "GetProjectsJSON")]
  public Task<List<Projects>> Get() => ProjectsService.Get();
}
