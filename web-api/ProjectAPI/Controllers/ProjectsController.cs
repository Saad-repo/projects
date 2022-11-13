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
    public async Task<IActionResult> GetAll([FromQuery] string activationCode)
    {
        var projects = await projectsService.GetProjects(activationCode);
        return Ok(projects);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetProjectStatusSummary([FromQuery] string activationCode)
    {
        var projects = await projectsService.GetProjectStatusSummary(activationCode);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (id.Length < 9)
            return ValidationProblem();

        int count = await this.projectsService.DeleteProject(id);
        return Ok("{\"count\": " + count.ToString() + " }");
    }


    [HttpPut("{actionCode}/{encodedId}")]
    public async Task<IActionResult> Approve([FromRoute] string actionCode, [FromRoute] string encodedId)
    {
        if (encodedId.Length < 9)
            return ValidationProblem();

        int count = 0;
        if (actionCode == "Submit")
            count = await this.projectsService.ApproveProject(encodedId, "Submitted");
        else if (actionCode == "Approve")
            count = await this.projectsService.ApproveProject(encodedId, "Approved");
        return Ok("{\"count\": " + count.ToString() + " }");
    }
}
