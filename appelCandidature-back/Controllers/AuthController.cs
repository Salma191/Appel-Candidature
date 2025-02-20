using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pfe_back.Data;
using pfe_back.DTOs;
using pfe_back.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using System.Text;

namespace pfe_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST : api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Vérifier si l'utilisateur existe déjà
            if (await _context.Utilisateurs.AnyAsync(u => u.Email == model.Email))
                return BadRequest("Cet email est déjà utilisé.");

            var roleCandidat = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == "Candidat");
            if (roleCandidat == null)
            {
                return BadRequest("Le rôle 'Candidat' n'existe pas.");
            }

            // Création de l'utilisateur
            var utilisateur = new Utilisateur
            {
                Nom = model.Nom,
                Prenom = model.Prenom,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RoleId = roleCandidat.Id
            };

            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur enregistré avec succès !");
        }

        // POST : api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (utilisateur == null || !BCrypt.Net.BCrypt.Verify(model.Password, utilisateur.Password))
                return Unauthorized("Email ou mot de passe incorrect.");

            var token = GenerateJwtToken(utilisateur);

            return Ok(new { Token = token });
        }

        // Méthode pour générer le token JWT
        private string GenerateJwtToken(Utilisateur utilisateur)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email),
                new Claim(ClaimTypes.Role, utilisateur.Role.Nom)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
