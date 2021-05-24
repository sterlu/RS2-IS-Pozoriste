using System.Text;
using Server.Interfaces;
using Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;


namespace Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(Korisnik korisnik)
        {
            var claims = new List<Claim>
            {
                new Claim("username", korisnik.Username),
                new Claim("tip", korisnik.Tip)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(15), 
                SigningCredentials = creds
            };

            var tokenHendler = new JwtSecurityTokenHandler();

            var token = tokenHendler.CreateToken(tokenDescriptor);

            return tokenHendler.WriteToken(token); 
        }
    }
}