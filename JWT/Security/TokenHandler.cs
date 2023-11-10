﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace JWT.Security
{
    public class TokenHandler
    {
        public static Token CreateToken(IConfiguration configuration)
        {
            Token token = new();//token nesnesimizi türetik

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.Now.AddMinutes(Convert.ToInt16(configuration["Token:Expiration"]));
              JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration["Token:Issuer"],
                audience: configuration["Token:Audience"],
                expires: token.Expiration,
                notBefore:DateTime.Now,//now dan önce başlamasın
                signingCredentials:credentials
                );
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
            //burası refresh token için yazıyoruz bu algoritmayı 
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random =RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            token.RefreshToken=Convert.ToBase64String(numbers);
            return token;
        }

    }
}
