﻿using BlogAPI.Data;
using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Models.Dto;
using BlogAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string _secretKey;
        public UserRepository(ApplicationDbContext context, IConfiguration configuration) : base(context) 
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        //public bool IsUiniqueUser(string username)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.UserName == username);

        //    if (user == null) {
        //        return true;
        //    }

        //    return false;
        //}

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
			var user = _context.Users.FirstOrDefault(u => u.Email == loginRequestDTO.Email);

			if (user == null)
			{
				return new LoginResponseDTO()
				{
					User = null,
					Token = ""
				};
			}

			// Initialize Password Helper
			var passwordHelper = new PasswordHelper();

			// Verify the provided password against the stored hashed password
			bool isValidPassword = passwordHelper.VerifyPassword(user.HashPassword, loginRequestDTO.HashPassword);

			if (!isValidPassword)
			{
				return new LoginResponseDTO()
				{
					User = null,
					Token = ""
				};
			}

			//         var passwordHelper = new PasswordHelper();
			//         //var hashedPassword = passwordHelper.HashPassword(loginRequestDTO.HashPassword);

			//         var user = _context.Users.FirstOrDefault(u=> u.Email == loginRequestDTO.Email);
			//         bool isValidPassword = false;
			//if (user != null) {
			//	isValidPassword = passwordHelper.VerifyPassword(loginRequestDTO.HashPassword, user.HashPassword);
			//}
			//if (user == null || isValidPassword == false) {
			//             return new LoginResponseDTO()
			//             {
			//                 User = null,
			//                 Token = ""
			//             };
			//         }

			// IF user found => Generate JWT Token
			var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return loginResponseDTO;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
