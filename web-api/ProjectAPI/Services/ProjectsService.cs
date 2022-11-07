using AbiClassLib.ProjectDb;
using Microsoft.VisualBasic;
using ProjectAPI.Models;
using System.Reflection;
using SDKLibV6.Functionality;

public interface IProjectsService
{
    Task<IEnumerable<ProjectSubmit>> GetProjects();
    Task<int> UpsertProject(ProjectSubmit prj);
}

public class ProjectsService : IProjectsService
{
    public async Task<IEnumerable<ProjectSubmit>> GetProjects()
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.GetProjectSubmits();
        return await Task.Run(() => prjs);
    }

    public async Task<int> UpsertProject(ProjectSubmit prj)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var prjs = sqLiteOps.SaveProjectSubmit(prj);
        return await Task.Run(() => prjs);
    }
}
