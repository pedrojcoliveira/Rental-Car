using System.ComponentModel.DataAnnotations;
using Rental_Car_Project.Models;

namespace Rental_Car_Project.Tests
{

    // Testes das regras de validação do veículo.

    public class VehicleValidationTests
    {
        // Cria um veículo válido; cada teste altera apenas o campo que quer testar
        private static Vehicle ValidVehicle() => new()
        {
            Brand = "Renault",
            Model = "Clio",
            LicensePlate = "AA-00-BB",
            ManufactureYear = 2020,
            FuelType = FuelType.Diesel
        };

        // Corre as mesmas validações que o ASP.NET corre ao submeter o formulário:
        // os atributos ([Required], [StringLength]...) e o método Validate do Model
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
        public void ValidVehicle_HasNoErrors()
        {
            // Arrange
            var vehicle = ValidVehicle();

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.Empty(results);
        }

        // ---------- Ano de fabrico ----------

        [Fact]
        public void CurrentYear_IsValid()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.ManufactureYear = DateTime.Today.Year;

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void NextYear_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.ManufactureYear = DateTime.Today.Year + 1;

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.ManufactureYear)));
        }

        [Fact]
        public void YearBefore1900_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.ManufactureYear = 1899;

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.ManufactureYear)));
        }

        [Fact]
        public void MissingManufactureYear_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.ManufactureYear = null;

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.ManufactureYear)));
        }

        // ---------- Campos obrigatórios ----------

        [Fact]
        public void MissingBrand_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.Brand = "";

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.Brand)));
        }

        [Fact]
        public void MissingLicensePlate_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.LicensePlate = "";

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.LicensePlate)));
        }

        [Fact]
        public void MissingFuelType_IsRejected()
        {
            // Arrange
            var vehicle = ValidVehicle();
            vehicle.FuelType = null;

            // Act
            var results = Validate(vehicle);

            // Assert
            Assert.True(HasErrorFor(results, nameof(Vehicle.FuelType)));
        }
    }
}