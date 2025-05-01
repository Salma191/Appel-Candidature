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
using pfe_back.Services;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace pfe_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailService mailService;


        public AuthController(ApplicationDbContext context, IConfiguration configuration, IMailService mailService)
        {
            _context = context;
            _configuration = configuration;
            this.mailService = mailService;
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


            var randomAffectation = await _context.Entites
                    .OrderBy(o => Guid.NewGuid())
                    .Select(o => o.Nom)
                    .FirstOrDefaultAsync();

            var randomPoste = await _context.Poste
                .OrderBy(o => Guid.NewGuid())
                .Select(o => o.Description)
                .FirstOrDefaultAsync();



            var candidat = new Candidat
            {
                AffectationActuelle = randomAffectation ?? "Affectation Test",
                DateRetraite = DateTime.Now.AddYears(5 + new Random().Next(0, 5)),
                JoursAbsence = new Random().Next(0, 10),
                Sanction = "Aucune",
                NoteTroisDernieresAnnees = "Non disponible",
                Catégorie = string.Empty,
                Congé = "Non",
                PosteOccupe = randomPoste ?? "Poste Test",
                Consentement = false,
                UtilisateurId = utilisateur.Id,
                Utilisateur = utilisateur,
                EmailVerificationToken = Guid.NewGuid().ToString(),
                TokenGeneratedAt = DateTime.UtcNow
            };


            _context.Candidats.Add(candidat);
            await _context.SaveChangesAsync();

            var verificationUrl = $"{_configuration["Jwt:Audience"]}/verify-email?token={candidat.EmailVerificationToken}";
            var request = new WelcomeRequest
            {
                ToEmail = utilisateur.Email,
                UserName = utilisateur.Nom,
                VerificationUrl = verificationUrl
            };

            await mailService.SendWelcomeEmailAsync(request);

        
            return Ok(new { Message = "Inscription réussie. Veuillez vérifier votre email.", Token = candidat.EmailVerificationToken });
        }


        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var candidat = await _context.Candidats.FirstOrDefaultAsync(c => c.EmailVerificationToken == token);

            if (candidat == null || candidat.Statut == true)
                return BadRequest("Token invalide ou déjà utilisé.");

            candidat.Statut = true;
            candidat.EmailVerificationToken = null;
            await _context.SaveChangesAsync();

            return Ok("Email vérifié avec succès.");
        }


        [HttpPost("recheck/{id}")]
        public async Task<IActionResult> SendWelcomeMail(int id)
        {
            try
            {
                var candidat = await _context.Candidats
                    .Include(c => c.Utilisateur)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (candidat == null || candidat.Utilisateur == null)
                {
                    return NotFound(new { Message = "Candidat ou utilisateur introuvable." });
                }

                candidat.EmailVerificationToken = Guid.NewGuid().ToString();
                await _context.SaveChangesAsync();

                var verificationUrl = $"{_configuration["Jwt:Audience"]}/verify-email?token={candidat.EmailVerificationToken}";
                var request = new WelcomeRequest
                {
                    ToEmail = candidat.Utilisateur.Email,
                    UserName = candidat.Utilisateur.Nom,
                    VerificationUrl = verificationUrl
                };

                await mailService.SendWelcomeEmailAsync(request);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue.", Details = ex.Message });
            }
        }

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
                .Include(u => u.Candidat)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (utilisateur == null || !BCrypt.Net.BCrypt.Verify(model.Password, utilisateur.Password))
            {
                Console.WriteLine("Échec d'authentification : utilisateur non trouvé ou mot de passe incorrect.");
                return Unauthorized(new { Message = "Email ou mot de passe incorrect." });
            }

            if (utilisateur.Candidat != null && utilisateur.Candidat.Statut == false)
            {
                return Unauthorized(new { Message = "Veuillez vérifier votre adresse email avant de vous connecter." });
            }

            var token = GenerateJwtToken(utilisateur);
            Console.WriteLine($"Token généré: {token}");

            return Ok(new { Token = token, utilisateur.Prenom, utilisateur.Role?.Nom, utilisateur.Id });
        }


        // Méthode pour générer le token JWT
        private string GenerateJwtToken(Utilisateur utilisateur)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email),
                new Claim(ClaimTypes.Role, utilisateur.Role.Nom),

            };

            if (utilisateur.Role.Nom == "Candidat" && utilisateur.Candidat != null)
            {
                claims.Add(new Claim("candidatId", utilisateur.Candidat.Id.ToString()));
            }

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
