using Business.Abstract;
using Core.Result;
using Core.Security.Encryptions;
using DataAccess.Abstract;
using Entity.DTO;
using Entity.FTM;
using MelofyAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserDataAccessService userDataAccessService;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        private const string BearerPrefix = "Bearer ";

        public AuthenticationService(IUserDataAccessService userDataAccessService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.userDataAccessService = userDataAccessService;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        private JwtSecurityToken CreateToken(List<Claim> claims)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                this.configuration["ApplicationSettings:JwT:Secret"]!));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            _ = int.TryParse(this.configuration["ApplicationSettings:JwT:TokenValidityInMinutes"], out int tokenValidityInMin);

            var token = new JwtSecurityToken(
                issuer: this.configuration["ApplicationSettings:JwT:ValidIssuer"],
                audience: this.configuration["ApplicationSettings:JwT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMin),
                claims: claims,
                signingCredentials: signingCredentials
                );
            return token;
        }

        public UserIdentityData ReadUserIdentityData()
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            
            this.httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues sv);

            JwtPayload payload = handler.ReadJwtToken(sv.ToString().Substring(BearerPrefix.Length)).Payload;
            UserIdentityData data = new UserIdentityData
            {
                Sid = Int32.Parse(payload[JwtRegisteredClaimNames.Sid].ToString()!),
                Email = payload[JwtRegisteredClaimNames.Email].ToString(),
                Name = payload[JwtRegisteredClaimNames.Name].ToString(),
                Role = payload[ClaimTypes.Role].ToString(),
            };

            return data;
        }

        public async Task<FormResult<object?>> RegisterUser(RegisterRequest register)
        {
            try
            {
                FormFieldGroup group = new FormFieldGroup(
                    new FormField("email", register.Email,
                        new Melofy(Melofies.Required, "Email is required."),
                        new Melofy(Melofies.Email, "Email is invalid."))
                    .AddNormalization(Normalizations.NormalizeTrim)
                    .AddNormalization(Normalizations.NormalizeLowerCase),
                    new FormField("name", register.Name,
                        new Melofy(Melofies.Required, "Name is required."),
                        new Melofy((control) => Melofies.Pattern(control, RegexPattern.FullName), "Name is invalid."))
                    .AddNormalization(Normalizations.NormalizeTrim)
                    .AddNormalization(Normalizations.NormalizeCapitalizeFirstLetterWords),
                    new FormField("password", register.Password,
                        new Melofy(Melofies.Required, "Password is required."),
                        new Melofy((control) => Melofies.MinLength(control, 8), "Password must be 8 character at least."),
                        new Melofy((control) => Melofies.MaxLength(control, 64), "Password could be 64 character at most."),
                        new Melofy((control) => Melofies.Pattern(control, RegexPattern.Password), "Password could not contain some special characters.")),
                    new FormField("repeat", register.PasswordRepeat,
                        new Melofy(Melofies.Required, "Password repeat is required"),
                        new Melofy((control) => Melofies.EqualiaventString(control, register.Password?.Content), "Password repeat does not match.")));

                group.Normalize();

                if (!group.Valid)
                    return new FormResult<object?>(false, null, "Register is failed.", group.Messages);

                string salt = Encryption.CreateSalt(64);
                string hash = Encryption.SHA512Hashing(register?.Password?.Content, salt);

                var exec = await this.userDataAccessService.RegisterUser((string)group.GetValue("email"), (string)group.GetValue("name"), hash, salt);
                if (exec == null)
                    return new FormResult<object?>(false, null, "Internal server error.", group.Messages);

                if (!exec.Success)
                {
                    group.Messages.Add(new MessageResult(false, "email", "Email is already available. Try to login."));
                    return new FormResult<object?>(false, null, "Email is already available. Try to login.", group.Messages);
                }

                return new FormResult<object?>(true, null, "Register successful.", group.Messages);
            }
            catch (Exception)
            {
                return new FormResult<object?>(false, null, "Internal server error.", null);
            }
        }
        public async Task<FormResult<string?>> LoginUser(LoginRequest login)
        {
            try
            {
                if (login == null || login.Email == null || login.Password == null)
                    return new FormResult<string?>(false, null, "Empty request.", null);

                FormFieldGroup group = new FormFieldGroup(
                    new FormField("email", login.Email,
                        new Melofy(Melofies.Required, "Email is required."),
                        new Melofy(Melofies.Email, "Email is invalid."))
                    .AddNormalization(Normalizations.NormalizeTrim)
                    .AddNormalization(Normalizations.NormalizeLowerCase),
                    new FormField("password", login.Password,
                        new Melofy(Melofies.Required, "Password is required."),
                        new Melofy((control) => Melofies.MinLength(control, 8), "Password must be 8 character at least."),
                        new Melofy((control) => Melofies.MaxLength(control, 64), "Password could be 64 character at most."),
                        new Melofy((control) => Melofies.Pattern(control, RegexPattern.Password), "Password could not contain some special characters.")));

                group.Normalize();
                if (!group.Valid)
                    return new FormResult<string?>(false, null, "Login failed.", group.Messages);

                var exec = await this.userDataAccessService.LoginUser((string)group.GetValue("email"));
                if (exec == null)
                {
                    group.Messages.Add(new MessageResult(false, login.Email.Code, "Account was not found."));
                    return new FormResult<string?>(false, null, "Login failed.", group.Messages);
                }

                string salt = exec.PasswordSalt;
                string hash = Encryption.SHA512Hashing(login.Password.Content, salt);
                if (hash != exec.PasswordHash)
                {
                    group.Messages.Add(new MessageResult(false, login.Password.Code, "Password wrong."));
                    return new FormResult<string?>(false, null, "Login failed.", group.Messages);
                }

                List<Claim> claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sid, exec.UserID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, exec.Email),
                    new Claim(JwtRegisteredClaimNames.Name, exec.Name),
                    new Claim(ClaimTypes.Role, "USER"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                string token = handler.WriteToken(this.CreateToken(claims));

                return new FormResult<string?>(true, token, "Login successful.", group.Messages);
            }
            catch (Exception)
            {
                return new FormResult<string?>(false, null, "Server internal error.", null);
            }
        }

    }
}
