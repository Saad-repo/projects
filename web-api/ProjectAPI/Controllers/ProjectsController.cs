using AbiClassLib.ProjectDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;
using ProjectAPI.Services;
using System.Net;
using System.Text.Json;

namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : Controller
{
    private readonly IProjectsService projectsService;
    private IUserService userService;

    public ProjectsController(IProjectsService projectsService, IUserService userService)
    {
        this.projectsService = projectsService;
        this.userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await projectsService.GetProjects();
        return Ok(projects);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] ProjectSubmit model)
    {
        int count = await this.projectsService.UpsertProject(model);
        string ipAddressString = $"{Request.HttpContext.Connection.RemoteIpAddress}  |  {Request.Headers["cl-id"]}";
        userService.RecordActivity(model.SubmitUser, ipAddressString, "Added/saved project: " + JsonSerializer.Serialize(model));
        return Ok("{\"count\": " + count.ToString() + " }");
    }

}
