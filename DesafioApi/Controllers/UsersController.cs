using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioApi.Data;
using DesafioApi.Entidades;
using DesafioApi.Config;
using DesafioApi.Dto;
using Microsoft.IdentityModel.Tokens;

namespace DesafioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context;

        public UsersController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusuarios()
        {
            return await _context.Users.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> login(UserDto user)
        {
            string token = "";
            var users = await _context.Users.ToListAsync();
            var usersBd = (from u in users
                           where u.Nome == user.UserName & u.Password == user.Password
                           select u).ToList();
            if (!usersBd.IsNullOrEmpty())
            {
                User userLogado = usersBd[0];
                token = TokenService.GenerateToken(userLogado);
            }

            return new { token = token };
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
