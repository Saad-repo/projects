using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AbiClassLib.ProjectDb;

public class ProjectDbOps
{

}

#region Dtos
public class ProjectUsers
{
    public int? UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Description { get; set; }
    public string Roles { get; set; }
    public string ActivationCode { get; set; }
    public string CreateDt { get; set; }
    public int IsActive { get; set; }
}

public class ProjectSubmit
{
    public int? SubmitId { get; set; }
    public int SubmitUser { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public string ProjectLinks { get; set; }
    public string ProjectDate { get; set; }
    public double ProjectCost { get; set; }
    public string ProjectStates { get; set; }
    public string CreateDt { get; set; }
    public string UpdateDt { get; set; }
    public int UpdateUser { get; set; }
    public string Status { get; set; }
}

public class ProjectUserAuth
{
    public int AuthId { get; set; }
    public int UserId { get; set; }
    public string AuthCode { get; set; }
    public string CreateDt { get; set; }
    public string ExpiresDt { get; set; }
}

public class ProjectUserAccessHistory
{
    public int UserId { get; set; }
    public string AccessAgent { get; set; }
    public string IpAddress { get; set; }
    public string AccessDt { get; set; }
}
#endregion

public sealed class SqLiteOps
{
    private string _db3FilePath;
    private string Now => $"{DateTime.Now.ToUniversalTime():yyyy-MM-ddTHH:mm:ss.000Z}";

    /// <summary>
    ///     Ctor
    /// </summary>
    public SqLiteOps(string fullPathToDb3File)
    {
        _db3FilePath = fullPathToDb3File;
    }


    #region CRUD for [prj.project_users]
    public IEnumerable<ProjectUsers> GetProjectUserss()
    {
        var sqlSelect = @"SELECT 
                                     [user_id] as [UserId],
                                     [email] as [Email],
                                     [full_name] as [FullName],
                                     [user_name] as [UserName],
                                     [description] as [Description],
                                     [roles] as [Roles],
                                     [create_dt] as [CreateDt],
                                     [is_active] as [IsActive]
                                FROM [prj.project_users]
                            ORDER BY [user_id] DESC";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectUsers>(sqlSelect);
        return queryResult;
    }

    public int GetActiveUsersCountByEmail(string emailAddr)
    {
        var sqlSelect = @"SELECT count(*)
                            FROM [prj.project_users]
                           WHERE [email] = @email
                             and [is_active] = 1";
        var paramsObj = new { email = emailAddr };
        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<int>(sqlSelect, paramsObj);
        return queryResult.First();
    }


    public ProjectUsers? GetProjectUser(string userName, string authCode)
    {
        var  paramsObj = new { userName = userName, authCode = authCode };
        var sqlSelect = $@"SELECT 
                                u.[user_id] as [UserId],
                                u.[email] as [Email],
                                u.[full_name] as [FullName],
                                u.[user_name] as [UserName],
                                u.[description] as [Description],
                                u.[roles] as [Roles],
                                u.[create_dt] as [CreateDt],
                                u.[is_active] as [IsActive]
                        FROM [prj.project_users] u
                    INNER JOIN [prj.project_user_auth] a on a.user_id = u.user_id
                        WHERE a.expires_dt > '{Now}'
                            and u.is_active = 1
                            and u.email = @userName
                            and a.auth_code = @authCode
                    ORDER BY u.[user_id] DESC
                        LIMIT 1;
";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectUsers>(sqlSelect, paramsObj);
        return queryResult.FirstOrDefault();
    }

    public int GetProjectUserIdToRegister(string userName, string activationCode)
    {
        var paramsObj = new { userName = userName, activationCode = activationCode };
        var sqlSelect = $@"SELECT [user_id]
                             FROM [prj.project_users]
                           WHERE  email = @userName
                              and activation_code = @activationCode
                    ORDER BY [user_id] DESC
                        LIMIT 1;
";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<int>(sqlSelect, paramsObj);
        return queryResult.FirstOrDefault();
    }

    public ProjectUsers? GetProjectUserByActivationCode(string activationCode)
    {
        var paramsObj = new { activationCode = activationCode };
        var sqlSelect = @"SELECT 
                                     [user_id] as [UserId],
                                     [email] as [Email],
                                     [full_name] as [FullName],
                                     [user_name] as [UserName],
                                     [description] as [Description],
                                     [roles] as [Roles],
                                     [activation_code] as [ActivationCode],
                                     [create_dt] as [CreateDt],
                                     [is_active] as [IsActive]
                                FROM [prj.project_users]
                               WHERE [activation_code] = @activationCode
                                 AND [is_active] = 0";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectUsers>(sqlSelect, paramsObj);
        return queryResult.FirstOrDefault();
    }


    public int RecordActivity(int userId, string ip, string userAgent)
    {
        var sqlPersist = @"INSERT INTO [prj.project_user_access_history] 
(user_id, access_agent, ip_address, access_dt)
VALUES
(@user_id, @access_agent, @ip_address, @access_dt)";
//        (1, 'chrome', 'ip-address', '2022-10-17T01:16:50.241Z')

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var persistCmd = GetDbCommand(conn, sqlPersist);
        persistCmd.Parameters.Add(GetDataParameter("@user_id", userId));
        persistCmd.Parameters.Add(GetDataParameter("@access_agent", userAgent));
        persistCmd.Parameters.Add(GetDataParameter("@ip_address", ip));
        persistCmd.Parameters.Add(GetDataParameter("@access_dt", Now));
        return persistCmd.ExecuteNonQuery();
    }



    public ProjectUsers SaveProjectUsers(ProjectUsers prjProjectUsers)
    {
        var sqlPersist = @"INSERT or REPLACE INTO [prj.project_users] 
                                ([user_id], [email], [full_name], [user_name], [description], [roles], [activation_code], [create_dt], [is_active])
                              VALUES 
                                (@user_id, @email, @full_name, @user_name, @description, @roles, @activation_code, @create_dt, @is_active)";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var persistCmd = GetDbCommand(conn, sqlPersist);
        persistCmd.Parameters.Add(GetDataParameter("@user_id", prjProjectUsers.UserId));
        persistCmd.Parameters.Add(GetDataParameter("@email", prjProjectUsers.Email));
        persistCmd.Parameters.Add(GetDataParameter("@full_name", prjProjectUsers.FullName));
        persistCmd.Parameters.Add(GetDataParameter("@user_name", prjProjectUsers.UserName));
        persistCmd.Parameters.Add(GetDataParameter("@description", prjProjectUsers.Description));
        persistCmd.Parameters.Add(GetDataParameter("@roles", prjProjectUsers.Roles));
        persistCmd.Parameters.Add(GetDataParameter("@activation_code", prjProjectUsers.ActivationCode));
        persistCmd.Parameters.Add(GetDataParameter("@create_dt", prjProjectUsers.CreateDt));
        persistCmd.Parameters.Add(GetDataParameter("@is_active", prjProjectUsers.IsActive));

        int count = persistCmd.ExecuteNonQuery();
        return prjProjectUsers;
    }

    public int DeleteProjectUsers(ProjectUsers prjProjectUsers)
    {
        StringBuilder sb = new();
        var sqlDelete = $"DELETE FROM [prj.project_users] WHERE [user_id] = {prjProjectUsers.UserId}";

        using System.Data.IDbConnection conn = GetDbConnection();
        conn.Open();
        var deleteCmd = GetDbCommand(conn, sqlDelete);
        return deleteCmd.ExecuteNonQuery();
    }
    #endregion

    #region CRUD for [prj.project_submit]
    public IEnumerable<ProjectSubmit> GetProjectSubmits()
    {
        var sqlSelect = @"SELECT 
                                     [submit_id] as [SubmitId],
                                     [submit_user] as [SubmitUser],
                                     [project_name] as [ProjectName],
                                     [description] as [Description],
                                     [project_links] as [ProjectLinks],
                                     [project_date] as [ProjectDate],
                                     [project_cost] as [ProjectCost],
                                     [project_states] as [ProjectStates],
                                     [create_dt] as [CreateDt],
                                     [update_dt] as [UpdateDt],
                                     [update_user] as [UpdateUser],
                                     [status] as [Status]
                                FROM [prj.project_submit]
                            ORDER BY [submit_id] DESC";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectSubmit>(sqlSelect);
        return queryResult;
    }

    public int SaveProjectSubmit(ProjectSubmit prjProjectSubmit)
    {
        var sqlPersist = @"INSERT or REPLACE INTO [prj.project_submit] 
                                ([submit_id], [submit_user], [project_name], [description], [project_links], [project_date], [project_cost], [project_states], [create_dt], [update_dt], [update_user], [status])
                              VALUES 
                                (@submit_id, @submit_user, @project_name, @description, @project_links, @project_date, @project_cost, @project_states, @create_dt, @update_dt, @update_user, @status)";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var persistCmd = GetDbCommand(conn, sqlPersist);
        persistCmd.Parameters.Add(GetDataParameter("@submit_id", prjProjectSubmit.SubmitId));
        persistCmd.Parameters.Add(GetDataParameter("@submit_user", prjProjectSubmit.SubmitUser));
        persistCmd.Parameters.Add(GetDataParameter("@project_name", prjProjectSubmit.ProjectName));
        persistCmd.Parameters.Add(GetDataParameter("@description", prjProjectSubmit.Description));
        persistCmd.Parameters.Add(GetDataParameter("@project_links", prjProjectSubmit.ProjectLinks));
        persistCmd.Parameters.Add(GetDataParameter("@project_date", prjProjectSubmit.ProjectDate));
        persistCmd.Parameters.Add(GetDataParameter("@project_cost", prjProjectSubmit.ProjectCost));
        persistCmd.Parameters.Add(GetDataParameter("@project_states", prjProjectSubmit.ProjectStates));
        persistCmd.Parameters.Add(GetDataParameter("@create_dt", prjProjectSubmit.CreateDt));
        persistCmd.Parameters.Add(GetDataParameter("@update_dt", prjProjectSubmit.UpdateDt));
        persistCmd.Parameters.Add(GetDataParameter("@update_user", prjProjectSubmit.UpdateUser));
        persistCmd.Parameters.Add(GetDataParameter("@status", prjProjectSubmit.Status));

        return persistCmd.ExecuteNonQuery();
    }

    public int DeleteProjectSubmit(ProjectSubmit prjProjectSubmit)
    {
        StringBuilder sb = new();
        var sqlDelete = $"DELETE FROM [prj.project_submit] WHERE [submit_id] = {prjProjectSubmit.SubmitId}";

        using System.Data.IDbConnection conn = GetDbConnection();
        conn.Open();
        var deleteCmd = GetDbCommand(conn, sqlDelete);
        return deleteCmd.ExecuteNonQuery();
    }
    #endregion

    #region CRUD for [prj.project_user_auth]
    public IEnumerable<ProjectUserAuth> GetProjectUserAuths()
    {
        var sqlSelect = @"SELECT 
                                     [auth_id] as [AuthId],
                                     [user_id] as [UserId],
                                     [auth_code] as [AuthCode],
                                     [create_dt] as [CreateDt],
                                     [expires_dt] as [ExpiresDt]
                                FROM [prj.project_user_auth]
                            ORDER BY [auth_id] DESC";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectUserAuth>(sqlSelect);
        return queryResult;
    }

    public int SaveProjectUserAuth(ProjectUserAuth prjProjectUserAuth)
    {
        var sqlPersist = @"INSERT or REPLACE INTO [prj.project_user_auth] 
                                ([auth_id], [user_id], [auth_code], [create_dt], [expires_dt])
                              VALUES 
                                (@auth_id, @user_id, @auth_code, @create_dt, @expires_dt)";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var persistCmd = GetDbCommand(conn, sqlPersist);
        persistCmd.Parameters.Add(GetDataParameter("@auth_id", prjProjectUserAuth.AuthId));
        persistCmd.Parameters.Add(GetDataParameter("@user_id", prjProjectUserAuth.UserId));
        persistCmd.Parameters.Add(GetDataParameter("@auth_code", prjProjectUserAuth.AuthCode));
        persistCmd.Parameters.Add(GetDataParameter("@create_dt", prjProjectUserAuth.CreateDt));
        persistCmd.Parameters.Add(GetDataParameter("@expires_dt", prjProjectUserAuth.ExpiresDt));

        return persistCmd.ExecuteNonQuery();
    }

    public int DeleteProjectUserAuth(ProjectUserAuth prjProjectUserAuth)
    {
        StringBuilder sb = new();
        var sqlDelete = $"DELETE FROM [prj.project_user_auth] WHERE [auth_id] = {prjProjectUserAuth.AuthId}";

        using System.Data.IDbConnection conn = GetDbConnection();
        conn.Open();
        var deleteCmd = GetDbCommand(conn, sqlDelete);
        return deleteCmd.ExecuteNonQuery();
    }
    #endregion

    #region CRUD for [prj.project_user_access_history]
    public IEnumerable<ProjectUserAccessHistory> GetProjectUserAccessHistorys()
    {
        var sqlSelect = @"SELECT 
                                     [user_id] as [UserId],
                                     [access_agent] as [AccessAgent],
                                     [ip_address] as [IpAddress],
                                     [access_dt] as [AccessDt]
                                FROM [prj.project_user_access_history]
                            ORDER BY [user_id] DESC";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var queryResult = conn.Query<ProjectUserAccessHistory>(sqlSelect);
        return queryResult;
    }

    public int SaveProjectUserAccessHistory(ProjectUserAccessHistory prjProjectUserAccessHistory)
    {
        var sqlPersist = @"INSERT or REPLACE INTO [prj.project_user_access_history] 
                                ([user_id], [access_agent], [ip_address], [access_dt])
                              VALUES 
                                (@user_id, @access_agent, @ip_address, @access_dt)";

        using IDbConnection conn = GetDbConnection();
        conn.Open();
        var persistCmd = GetDbCommand(conn, sqlPersist);
        persistCmd.Parameters.Add(GetDataParameter("@user_id", prjProjectUserAccessHistory.UserId));
        persistCmd.Parameters.Add(GetDataParameter("@access_agent", prjProjectUserAccessHistory.AccessAgent));
        persistCmd.Parameters.Add(GetDataParameter("@ip_address", prjProjectUserAccessHistory.IpAddress));
        persistCmd.Parameters.Add(GetDataParameter("@access_dt", prjProjectUserAccessHistory.AccessDt));

        return persistCmd.ExecuteNonQuery();
    }

    public int DeleteProjectUserAccessHistory(ProjectUserAccessHistory prjProjectUserAccessHistory)
    {
        StringBuilder sb = new();
        var sqlDelete = $"DELETE FROM [prj.project_user_access_history] WHERE [user_id] = {prjProjectUserAccessHistory.UserId}";

        using System.Data.IDbConnection conn = GetDbConnection();
        conn.Open();
        var deleteCmd = GetDbCommand(conn, sqlDelete);
        return deleteCmd.ExecuteNonQuery();
    }
    #endregion

    #region Helper methods
    private System.Data.IDbConnection GetDbConnection()
    {
        return new SqliteConnection($"Data Source=\"{_db3FilePath}\";");
    }

    private IDbCommand GetDbCommand(IDbConnection conn, string sqlText)
    {
        return new SqliteCommand(sqlText, (SqliteConnection)conn);
    }

    private IDbDataParameter GetDataParameter(string name, object value)
    {
        if (value is null)
            return new SqliteParameter(name, DBNull.Value);

        return new SqliteParameter(name, value);
    }

    #endregion
}