using System.ComponentModel.DataAnnotations;
using Rental_Car_Project.Models;

namespace Rental_Car_Project.Tests
{

    // Testes das regras de validação do contrato de aluguer.
     
    public class RentalContractValidationTests
    {
        private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

        // Cria um contrato válido; cada teste altera apenas o campo que quer testar
        private static RentalContract ValidContract() => new()
        {
            ClientId = 1,
            VehicleId = 1,
            StartDate = Today,
            EndDate = Today.AddDays(3),
            InitialMileage = 15000
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
        public void ValidContract_HasNoErrors()
        {
            // Arrange
            var contract = ValidContract();

            // Act
            var results = Validate(contract);

            // Assert
            Assert.Empty(results);
        }

        // ---------- Datas ----------

        [Fact]
        public void StartDateToday_IsValid()
        {
            // Arrange
            var contract = ValidContract();
            contract.StartDate = Today;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void StartDateInThePast_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.StartDate = Today.AddDays(-1);

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.StartDate)));
        }

        [Fact]
        public void EndDateEqualToStartDate_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.EndDate = contract.StartDate;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.EndDate)));
        }

        [Fact]
        public void EndDateBeforeStartDate_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.StartDate = Today.AddDays(5);
            contract.EndDate = Today.AddDays(2);

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.EndDate)));
        }

        // ---------- Quilometragem ----------

        [Fact]
        public void ZeroMileage_IsValid()
        {
            // Arrange
            var contract = ValidContract();
            contract.InitialMileage = 0;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void NegativeMileage_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.InitialMileage = -1;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.InitialMileage)));
        }

        // ---------- Campos obrigatórios ----------

        [Fact]
        public void MissingClient_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.ClientId = null;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.ClientId)));
        }

        [Fact]
        public void MissingVehicle_IsRejected()
        {
            // Arrange
            var contract = ValidContract();
            contract.VehicleId = null;

            // Act
            var results = Validate(contract);

            // Assert
            Assert.True(HasErrorFor(results, nameof(RentalContract.VehicleId)));
        }
    }
}