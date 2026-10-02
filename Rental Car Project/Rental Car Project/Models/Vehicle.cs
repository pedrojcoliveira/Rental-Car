using System.ComponentModel.DataAnnotations;

namespace Rental_Car_Project.Models
{
    public class Vehicle : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(50, ErrorMessage = "A marca não pode ter mais de 50 caracteres.")]
        [Display(Name = "Marca")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O modelo não pode ter mais de 50 caracteres.")]
        [Display(Name = "Modelo")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "A matrícula é obrigatória.")]
        [StringLength(8, ErrorMessage = "A matrícula não pode ter mais de 8 caracteres.")]
        [Display(Name = "Matrícula")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de fabrico é obrigatório.")]
        [Display(Name = "Ano de Fabrico")]
        public int? ManufactureYear { get; set; }

        [Required(ErrorMessage = "O tipo de combustível é obrigatório.")]
        [Display(Name = "Tipo de Combustível")]
        public FuelType? FuelType { get; set; }

        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int currentYear = DateTime.Today.Year;

            if (ManufactureYear > currentYear)
                yield return new ValidationResult(
                    $"O ano de fabrico não pode ser posterior a {currentYear}.",
                    new[] { nameof(ManufactureYear) });

            if (ManufactureYear < 1900)
                yield return new ValidationResult(
                    "O ano de fabrico não pode ser anterior a 1900.",
                    new[] { nameof(ManufactureYear) });
        }

    }

}