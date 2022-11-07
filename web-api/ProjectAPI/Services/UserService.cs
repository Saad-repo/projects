using AbiClassLib.ProjectDb;
using Microsoft.VisualBasic;
using ProjectAPI.Models;
using System.Reflection;
using SDKLibV6.Functionality;

namespace ProjectAPI.Services;

public interface IUserService
{
    Task<string> Singup(UserAuthModelSignup model);
    Task<ProjectUsers?> ConfirmSingup(string activationCode);
    Task<ProjectUsers?> Authenticate(string username, string password);
    Task<IEnumerable<ProjectUsers>> GetAll();

    bool IsActiveUser(int userId);

    int RecordActivity(int userId, string ip, string userAgent);
    Task<int> GetActiveUserCountByEmail(string email);
}

public class UserService : IUserService
{
    public async Task<ProjectUsers?> Authenticate(string username, string password)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");

        var user = await Task.Run(() => sqLiteOps.GetProjectUser( username , password));

        return user;
    }

    public async Task<string> Singup(UserAuthModelSignup model)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");

        Cryptography cryptography = new Cryptography();
        string activationCode = cryptography.GeneratePasword(8, PwdCategories.UpperCase | PwdCategories.Numbers | PwdCategories.LowerCase);

        ProjectUsers prjProjectUser = new()
        { 
            CreateDt = Util.Now,
            Description = "description",
            Email = model.Email,
            FullName = model.FullName,
            IsActive = 0,
            Roles = "Default",
            UserId = null,
            ActivationCode = activationCode,
            UserName = model.Username
        };
        var usr = sqLiteOps.SaveProjectUsers(prjProjectUser);
        var userId = sqLiteOps.GetProjectUserIdToRegister(model.Username, activationCode);

        ProjectUserAuth prjProjectUserAuth = new()
        {
            UserId = userId,
            AuthCode = model.Password,
            CreateDt = Util.Now,
            ExpiresDt = Util.NowPlusYears(2),
        };

        var count = await Task.Run(() => sqLiteOps.SaveProjectUserAuth(prjProjectUserAuth));
        return activationCode;
    }

    public async Task<IEnumerable<ProjectUsers>> GetAll()
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var users = sqLiteOps.GetProjectUserss();
        return await Task.Run(() => users);
    }

    public async Task<int> GetActiveUserCountByEmail(string email)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        var count = sqLiteOps.GetActiveUsersCountByEmail(email);
        return await Task.Run(() => count);
    }

    public bool IsActiveUser(int userId)
    {
        throw new NotImplementedException();
    }

    public int RecordActivity(int userId, string ip, string userAgent)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");
        int count = sqLiteOps.RecordActivity(userId, ip, userAgent);
        return count;
    }

    public async Task<ProjectUsers?> ConfirmSingup(string activationCode)
    {
        SqLiteOps sqLiteOps = new(@"C:\SDK\MyData\Abis_DB.db3");

        var user = sqLiteOps.GetProjectUserByActivationCode(activationCode);
        if(user is not null && user.IsActive == 0)
        {
            user.IsActive = 1;
            sqLiteOps.SaveProjectUsers(user);
            return await Task.Run(() => sqLiteOps.SaveProjectUsers(user));
        }

        ProjectUsers? obj = null;
        return await Task.Run(() => obj);
    }
}

