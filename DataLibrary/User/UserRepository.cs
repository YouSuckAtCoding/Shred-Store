using Dapper;
using DataLibrary.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.User
{
    public class UserRepository : IUserRepository
    {

        private readonly ISqlDataAccess sqlDataAccess;

        public UserRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public Task<IEnumerable<UserModel>> GetUsuarios() => sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });

        public async Task<UserModel?> GetUser(int id)
        {
            var result = await sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUser_GetById", new { Id = id} );

            return result.FirstOrDefault();

        }


        public async Task<UserModel?> Login(string Name, string Password)
        {
            var p = new DynamicParameters();
            p.Add("@Name", Name);
            p.Add("@Password", Password);
            p.Add("@ResponseMessage","", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output);

            var user = await sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUser_Login", p);
            
            if(user.Any())
            {
                return user.FirstOrDefault();
            }
            else
                return null;
            
        }

        public Task InsertUser(UserRegisterModel user) =>
            sqlDataAccess.SaveData("dbo.spUser_Insert", new { user.Name, user.Email, user.Password, user.Role });

        public Task UpdateUser(UserModel user) => sqlDataAccess.SaveData("dbo.spUser_Update", new { user.Id, user.Name, user.Email });

        public Task DeleteUser(int id) => sqlDataAccess.SaveData("dbo.spUser_Delete", new { Id = id });


    }
}
