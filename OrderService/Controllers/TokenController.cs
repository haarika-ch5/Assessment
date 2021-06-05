using OrderService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly OrderContext _context;

        public TokenController(IConfiguration config, OrderContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerInfo _CustomerData)
        {

            if (_CustomerData != null && _CustomerData.Email != null && _CustomerData.Password != null)
            {
                var Customer = await GetCustomer(_CustomerData.Email, _CustomerData.Password);

                if (Customer != null)
                {
                    //create claims details based on the Customer information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", Customer.CustomerId.ToString()),
                    new Claim("FirstName", Customer.FirstName),
                    new Claim("LastName", Customer.LastName),
                    new Claim("CustomerName", Customer.CustomerName),
                    new Claim("Email", Customer.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<CustomerInfo> GetCustomer(string email, string password)
        {
            return await _context.CustomerInfo.FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
        }
    }
}