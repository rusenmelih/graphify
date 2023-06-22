using DataAccess.Abstract;
using Entity.DBM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Provider
{
    public class UserDataAccessProvider : IUserDataAccessService
    {
        public async Task<ProcedureResult?> RegisterUser(string email, string name, string hash, string salt)
        {
            using (GraphifyDb db = new GraphifyDb())
                return (await db.ProcedureResults.FromSqlRaw("EXEC Register @email, @name, @hash, @salt",
                    new SqlParameter("email", email),
                    new SqlParameter("name", name),
                    new SqlParameter("hash", hash),
                    new SqlParameter("salt", salt)).ToListAsync()).FirstOrDefault();
        }
        public async Task<UserCredential?> LoginUser(string email)
        {
            using (GraphifyDb db = new GraphifyDb())
                return (await db.UserCredentials.FromSqlRaw("EXEC GetUserCredentialsByEmail @email",
                    new SqlParameter("email", email)).ToListAsync()).FirstOrDefault();
        }
    }
}
