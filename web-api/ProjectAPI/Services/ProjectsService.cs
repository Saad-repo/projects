using AbiClassLib.ProjectDb;
using Microsoft.VisualBasic;
using ProjectAPI.Models;
using System.Reflection;
using SDKLibV6.Functionality;

public interface IProjectsService
{
    Task<IEnumerable<ProjectSubmit>> GetProjects(string activationCode);
    Task<IEnumerable<ProjectSubmitSummary>> GetProjectStatusSummary(string activationCode);
    Task<int> UpsertProject(ProjectSubmit prj);
    Task<int> DeleteProject(string encodedId);
    Task<int> ApproveProject(string encodedId, string actionCode);
}

public class ProjectsService : IProjectsService
{
    public async Task<IEnumerable<ProjectSubmit>> GetProjects(string activationCode)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.GetProjectSubmits(activationCode);
        return await Task.Run(() => prjs);
    }
    public async Task<IEnumerable<ProjectSubmitSummary>> GetProjectStatusSummary(string activationCode)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.GetProjectStatusSummary(activationCode);
        return await Task.Run(() => prjs);
    }

    public async Task<int> UpsertProject(ProjectSubmit prj)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.SaveProjectSubmit(prj);
        return await Task.Run(() => prjs);
    }

    public async Task<int> DeleteProject(string encodedId)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.DeleteProjectSubmit(encodedId);
        return await Task.Run(() => prjs);
    }

    public async Task<int> ApproveProject(string encodedId, string actionCode)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.UpdateStatusProjectSubmit(encodedId, actionCode);
        return await Task.Run(() => prjs);
    }
}
