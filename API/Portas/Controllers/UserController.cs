using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Portas.Data;
using Portas.Models;

namespace Portas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;

        public UserController(DataContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> Get()//hora hora, parece que aqui funcinou hsuahsuas xD
        {
            try
            {
                return await context.TBUser.ToListAsync();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<ActionResult> Create(User u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    u.senhaEncry = sha256encrypt(u.Senha);
                    await context.TBUser.AddAsync(u);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                    return BadRequest();

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ActionResult> Login(object _login)
        {
            User login = null;
            try
            {
                login = JsonConvert.DeserializeObject<User>(_login.ToString());
            }
            catch (Exception e)
            {

                return BadRequest();
            }

            if(login != null)
            {
                if (login.email != null && login.Senha != null)
                {
                    login.email = login.email.Trim();
                    login.Senha = login.Senha.Trim();

                    login.senhaEncry = sha256encrypt(login.Senha);
                    User l = await context.TBUser.FirstOrDefaultAsync(x => x.email == login.email && x.senhaEncry == login.senhaEncry);

                    if(l != null)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(Startup.Secret);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, l.nome),
                                new Claim(ClaimTypes.Email, l.email),
                                new Claim(ClaimTypes.Role, "Usuario Padrão")
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        login.Token = tokenHandler.WriteToken(token);

                        Dictionary<string, string> s = new Dictionary<string, string>();
                        s.Add("Nome", l.nome);
                        s.Add("Email", l.email);
                        s.Add("Token", login.Token);

                        return Ok(s);
                    }
                    
                }
            }
            return BadRequest();
        } 




        public static string sha256encrypt(string frase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(frase));
            //return BitConverter.ToString(hashedDataBytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedDataBytes.Length; i++)
            {
                builder.Append(hashedDataBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }
}
