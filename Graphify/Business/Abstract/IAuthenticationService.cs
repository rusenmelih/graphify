using Core.Result;
using Entity.DTO;
using Entity.FTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthenticationService
    {
        Task<FormResult<object?>> RegisterUser(RegisterRequest register);
        Task<FormResult<string?>> LoginUser(LoginRequest login);
        public UserIdentityData ReadUserIdentityData();
    }
}
