using pfe_back.Models;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace pfe_back.Test
{
    public class UtilisateurTest
    {
        [Fact]
        public void Utilisateur_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            // Arrange
            var utilisateur = new Utilisateur
            {
                Nom = "Doe",
                Prenom = "John",
                Email = "johndoe@example.com",
                Password = "ValidPassword123",
                RoleId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(utilisateur, new ValidationContext(utilisateur), validationResults, true);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void Utilisateur_ShouldFailValidation_WhenEmailIsInvalid()
        {
            // Arrange
            var utilisateur = new Utilisateur
            {
                Nom = "Doe",
                Prenom = "John",
                Email = "invalid-email",  // Invalid email format
                Password = "ValidPassword123",
                RoleId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(utilisateur, new ValidationContext(utilisateur), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().Contain(r => r.ErrorMessage == "The Email field is not a valid e-mail address.");
        }

        [Fact]
        public void Utilisateur_ShouldFailValidation_WhenPasswordIsTooShort()
        {
            // Arrange
            var utilisateur = new Utilisateur
            {
                Nom = "Doe",
                Prenom = "John",
                Email = "johndoe@example.com",
                Password = "short",  // Invalid password (too short)
                RoleId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(utilisateur, new ValidationContext(utilisateur), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().Contain(r => r.ErrorMessage == "Le mot de passe doit contenir au moins 8 caractères.");
        }

        [Fact]
        public void Utilisateur_ShouldFailValidation_WhenRequiredPropertiesAreMissing()
        {
            // Arrange
            var utilisateur = new Utilisateur
            {
                Nom = "Doe",
                Prenom = "John",
                Email = "johndoe@example.com",
                Password = "short",  // Invalid password (too short)
                RoleId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(utilisateur, new ValidationContext(utilisateur), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().Contain(r => r.ErrorMessage == "The Nom field is required.");
            validationResults.Should().Contain(r => r.ErrorMessage == "The Prenom field is required.");
            validationResults.Should().Contain(r => r.ErrorMessage == "The Email field is required.");
            validationResults.Should().Contain(r => r.ErrorMessage == "The Password field is required.");
        }
    }
}
