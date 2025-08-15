using DA.DB.Utils;
using DA.DB.Utils.Entities;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using DA.WI.NSGHSM.IRepo._Core.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo._Core.Configuration
{
    internal class UserRepo<TDataSource> : IUserRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public UserRepo(TDataSource dataSource)
        {

            this.ctx = dataSource.GetDbClient();

            //this.ctx = ctx;
            //ctx.Initialize(new DbConnectionInfo() { ConnectionString = dataSource.Config.ConnectionString });
        }

        public IEnumerable<UserDto> Read()
        {
            var result = new List<UserDto>();

            using (IDataReader reader = ctx.GetDataReader(Statements.Read))
            {

                while (reader.Read())
                {
                    MapRecordToUserDto(reader, ref result);
                }
            }

            return result;
        }

        public UserDto Read(long id)
        {
            var result = new List<UserDto>();
            using (IDataReader reader = ctx.GetDataReader(Statements.ReadFromId, ctx.CreateParameter("Id", id)))
            {

                while (reader.Read())
                {
                    MapRecordToUserDto(reader, ref result);
                }

                if (result.Count == 0)
                {
                    throw new NotFoundException(typeof(UserDto), id);
                }

            }
            return result[0];

        }

        public UserDto GetUserByName(string userName)
        {
            var result = new List<UserDto>();
            using (IDataReader reader = ctx.GetDataReader(Statements.ReadFromName, ctx.CreateParameter("Name", userName)))
            {

                while (reader.Read())
                {
                    MapRecordToUserDto(reader, ref result);
                }

                if (result.Count == 0)
                {
                    throw new NotFoundException(typeof(UserDto), userName);
                }

            }
            return result[0];
        }

        public UserDto Create(UserCreateDto dto)
        {
            ctx.ExecuteNonQuery(Statements.Create, ctx.CreateParameter("UserName", dto.UserName),
                ctx.CreateParameter("Password", "password"),
                ctx.CreateParameter("IsFromActiveDirectory", false));

            return Read(GetLastInsertRowId());
        }

        private long GetLastInsertRowId()
        {
            return ctx.GetEntity<long>(Statements.LastInsertRowId);
        }

        private void MapRecordToUserDto(IDataRecord r, ref List<UserDto> result)
        {
            long targetId = r.GetInt64(0);
            UserDto targetDto = result.FirstOrDefault(dto => dto.Id == targetId);
            if (targetDto == null)
            {
                targetDto = new UserDto
                {
                    Id = targetId,
                    UserName = r.GetString(1),
                    Password = r.GetString(2),
                    IsFromActiveDirectory = r.GetBoolean(3),
                    Roles = new List<string>()
                };
                result.Add(targetDto);
            };

            if (r.IsDBNull(4) == false)
            {
                string roleName = r.GetString(4);
                if (targetDto.Roles.Contains(roleName) == false)
                    targetDto.Roles.Add(roleName);
            }
        }

        private UserDto MapRecordToUserDto(IDataRecord r)
        {
            List<UserDto> result = new List<UserDto>();
            MapRecordToUserDto(r, ref result);
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }


        private static class Statements
        {

            public const string Read = @"SELECT DISTINCT  SecUsers.Id,
                                                          SecUsers.UserName,
                                                          SecUsers.Password,
                                                          SecUsers.IsFromActiveDirectory,
                                                          SecRoles.Name
                                                          FROM SecUsers
                                                          LEFT JOIN SecUsersRoles ON SecUsers.Id = SecUsersRoles.UserId
                                                          LEFT JOIN SecRoles ON SecUsersRoles.RoleId = SecRoles.Id

                                                          UNION

                                                          SELECT DISTINCT SecUsers.Id,
                                                                          SecUsers.UserName,
                                                                          SecUsers.Password,
                                                                          SecUsers.IsFromActiveDirectory,
                                                                          SecRoles.Name
                                                          FROM SecUsers
                                                          LEFT JOIN SecUsersGroups ON SecUsers.Id = SecUsersGroups.UserId
                                                          LEFT JOIN SecGroups ON SecUsersGroups.GroupId = SecGroups.Id
                                                          LEFT JOIN SecGroupsRoles ON SecGroups.Id = SecGroupsRoles.GroupId
                                                          LEFT JOIN SecRoles AS SecRoles ON SecGroupsRoles.RoleId = SecRoles.Id
                                                          ORDER BY SecUsers.Id
                                                  ";

            public const string ReadFromId =
                @"SELECT
                    s.*
                FROM
                    (
                    SELECT
                        DISTINCT SecUsers.Id,
                        SecUsers.UserName,
                        SecUsers.Password,
                        SecUsers.IsFromActiveDirectory,
                        SecRoles.Name
                    FROM
                        SecUsers
                    LEFT JOIN SecUsersRoles ON
                        SecUsers.Id = SecUsersRoles.UserId
                    LEFT JOIN SecRoles ON
                        SecUsersRoles.RoleId = SecRoles.Id
                    WHERE
                        (SecUsers.Id = :Id)
                UNION
                    SELECT
                        DISTINCT SecUsers.Id,
                        SecUsers.UserName,
                        SecUsers.Password,
                        SecUsers.IsFromActiveDirectory,
                        SecRoles.Name
                    FROM
                        SecUsers
                    LEFT JOIN SecUsersGroups ON
                        SecUsers.Id = SecUsersGroups.UserId
                    LEFT JOIN SecGroups ON
                        SecUsersGroups.GroupId = SecGroups.Id
                    LEFT JOIN SecGroupsRoles ON
                        SecGroups.Id = SecGroupsRoles.GroupId
                    LEFT JOIN SecRoles ON
                        SecGroupsRoles.RoleId = SecRoles.Id
                    WHERE
                        (SecUsers.Id = :Id)) s
                ORDER BY
                    s.Id";

            public const string ReadFromName =
                @"SELECT
                    s.*
                FROM
                    (
                    SELECT
                        DISTINCT SecUsers.Id,
                        SecUsers.UserName,
                        SecUsers.Password,
                        SecUsers.IsFromActiveDirectory,
                        SecRoles.Name
                    FROM
                        SecUsers
                    LEFT JOIN SecUsersRoles ON
                        SecUsers.Id = SecUsersRoles.UserId
                    LEFT JOIN SecRoles ON
                        SecUsersRoles.RoleId = SecRoles.Id
                    WHERE
                        (SecUsers.UserName = :Name)
                UNION
                    SELECT
                        DISTINCT SecUsers.Id,
                        SecUsers.UserName,
                        SecUsers.Password,
                        SecUsers.IsFromActiveDirectory,
                        SecRoles.Name
                    FROM
                        SecUsers
                    LEFT JOIN SecUsersGroups ON
                        SecUsers.Id = SecUsersGroups.UserId
                    LEFT JOIN SecGroups ON
                        SecUsersGroups.GroupId = SecGroups.Id
                    LEFT JOIN SecGroupsRoles ON
                        SecGroups.Id = SecGroupsRoles.GroupId
                    LEFT JOIN SecRoles ON
                        SecGroupsRoles.RoleId = SecRoles.Id
                    WHERE
                        (SecUsers.UserName = :Name)) s
                ORDER BY
                    s.Id";

            public const string Create = @"INSERT INTO
                                           SecUsers (UserName,
                                                     Password,
                                                     IsFromActiveDirectory)
                                           VALUES (:UserName,
                                                   :Password,
                                                   :IsFromActiveDirectory)";

            public const string LastInsertRowId = @"SELECT max (SecUsers.Id)
                                                    FROM SecUsers";



        };
    }


}
