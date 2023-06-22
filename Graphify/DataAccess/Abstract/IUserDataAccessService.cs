using Entity.DBM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDataAccessService
    {
        Task<ProcedureResult?> RegisterUser(string email, string name, string hash, string salt);
        Task<UserCredential?> LoginUser(string email);
    }
}
