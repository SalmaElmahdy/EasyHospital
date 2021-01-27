using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {

            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            //Error :we must set the prop of config prop of token key so we must go to => appsettings.Development
            // to declare "TokenKey" and put it in _key
            //_key used to in SigningCredentials to decalre key and security Algorithm
        }

        public string CreateToken(AppUser user)
        {
           var claims=new List<Claim>
           {
              new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
              //info about user like name,type,..
              //claims has many types like what we used above to claim just username

           };


           var creds=new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);
           //some info we must declare to protect our token

           var tokenDescriptor =new SecurityTokenDescriptor
           {
               Subject=new ClaimsIdentity(claims),
               Expires=DateTime.Now.AddDays(7),
               SigningCredentials=creds
           };
           //has info about the token whic the user used

           var tokenHandler =new JwtSecurityTokenHandler();
           var token=tokenHandler.CreateToken(tokenDescriptor);
           //has function which create token through the discrptor which has the attribute of token

           return tokenHandler.WriteToken(token);
           //return hash words

        }
    }
}