using System.ComponentModel.DataAnnotations;
using Rental_Car_Project.Models;

namespace Rental_Car_Project.Tests
{

    // Testes das regras de validação do cliente.

    public class ClientValidationTests
    {
        // Cria um cliente válido; cada teste altera apenas o campo que quer testar
        private static Client ValidClient() => new()
        {
            Name = "Pedro Oliveira",
            Email = "pedro@exemplo.pt",
            PhoneNumber = "912345678",
            DriverLicenseNumber = "P-1234567"
        };

        // Corre as mesmas validações que o ASP.NET corre ao submeter o formulário
        private static List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
            return results;
        }

        // Verifica se existe algum erro associado a um campo específico
        private static bool HasErrorFor(List<ValidationResult> results, string fieldName) =>
            results.Any(r => r.MemberNames.Contains(fieldName));

        [Fact]
        public void ValidClient_HasNoErrors()
        {
            // Arrange
            var client = ValidClient();

            // Act
            var results = Validate(client);

            // Assert
            Assert.Empty(results);
        }

        // ---------- Telefone ----------

        [Theory]
        [InlineData("912345678")] // móvel
        [InlineData("252123456")] // fixo
        public void ValidPhone_IsAccepted(string phone)
        {
            // Arrange
            var client = ValidClient();
            client.PhoneNumber = phone;

            // Act
            var results = Validate(client);

            // Assert
            Assert.Empty(results);
        }

        [Theory]
        [InlineData("91234567")]    // 8 dígitos
        [InlineData("9123456789")]  // 10 dígitos
        [InlineData("91234567a")]   // contém letra
        [InlineData("912 345 678")] // contém espaços
        [InlineData("812345678")]   // não começa por 2 nem 9
        public void InvalidPhone_IsRejected(string phone)
        {
            // Arrange
            var client = ValidClient();
            client.PhoneNumber = phone;

            // Act
            var results = Validate(client);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Client.PhoneNumber)));
        }

        // ---------- Email ----------

        [Theory]
        [InlineData("pedro")]
        [InlineData("pedro@")]
        [InlineData("@exemplo.pt")]
        public void InvalidEmail_IsRejected(string email)
        {
            // Arrange
            var client = ValidClient();
            client.Email = email;

            // Act
            var results = Validate(client);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Client.Email)));
        }

        // ---------- Campos obrigatórios ----------

        [Fact]
        public void MissingName_IsRejected()
        {
            // Arrange
            var client = ValidClient();
            client.Name = "";

            // Act
            var results = Validate(client);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Client.Name)));
        }

        [Fact]
        public void MissingDriverLicenseNumber_IsRejected()
        {
            // Arrange
            var client = ValidClient();
            client.DriverLicenseNumber = "";

            // Act
            var results = Validate(client);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Client.DriverLicenseNumber)));
        }
    }
}