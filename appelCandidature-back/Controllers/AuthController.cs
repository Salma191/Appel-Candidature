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
using Microsoft.AspNetCore.Authorization;

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
            await _context.SaveChangesAsync(); // Sauvegarde pour générer l'ID

            // Création du candidat lié à l'utilisateur
            var candidat = new Candidat
            {
                AffectationActuelle = string.Empty,  // Ou valeurs par défaut
                DateRetraite = DateTime.Now.AddYears(5),  // Valeur par défaut
                JoursAbsence = 0,  // Valeur par défaut
                Sanction = string.Empty,  // Valeur par défaut
                NoteTroisDernieresAnnees = string.Empty,  // Valeur par défaut
                Catégorie = string.Empty,  // Valeur par défaut
                Congé = string.Empty,  // Valeur par défaut
                PosteOccupe = string.Empty,  // Valeur par défaut
                Consentement = false,  // Valeur par défaut
                UtilisateurId = utilisateur.Id,  // Lier le candidat à l'utilisateur
                Utilisateur = utilisateur
            };

            // Ajouter le candidat à la base de données
            _context.Candidats.Add(candidat);
            await _context.SaveChangesAsync(); // Sauvegarde pour générer l'ID du candidat

            return Ok("Utilisateur et candidat enregistrés avec succès !");
        }




        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            Console.WriteLine($"Requête reçue avec email: {model?.Email}");
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { Message = "Email et mot de passe sont requis." });
            }

            var utilisateur = await _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (utilisateur == null || !BCrypt.Net.BCrypt.Verify(model.Password, utilisateur.Password))
            {
                Console.WriteLine("Échec d'authentification : utilisateur non trouvé ou mot de passe incorrect.");
                return Unauthorized(new { Message = "Email ou mot de passe incorrect." });
            }

            var token = GenerateJwtToken(utilisateur);
            Console.WriteLine($"Token généré: {token}");

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
