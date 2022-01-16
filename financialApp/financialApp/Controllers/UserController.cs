#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using financialApp.DAO;
using financialApp.DAO.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace financialApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly MyDbContext _context;

    public UserController(MyDbContext context)
    {
        _context = context;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(User user)
    {
        if (user == null)
        {
            return BadRequest("Invalid client request");
        }
        var dbUser = await _context.Users.Where(_ => _.Login == user.Login && _.Password == user.Password).FirstOrDefaultAsync();
        if (dbUser != default(User))
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new { Token = tokenString });
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(User user)
    {
        if (user == null)
        {
            return BadRequest("Invalid client request");
        }
        var isLoginUsed = await _context.Users.AnyAsync(_ => _.Login == user.Login);
        if (isLoginUsed)
        {
            return UnprocessableEntity("Login is taken");
        }
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok("Account created");
    }
}
