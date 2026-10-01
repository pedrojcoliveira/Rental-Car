using System.ComponentModel.DataAnnotations;

namespace Rental_Car_Project.Models
{
    public class RentalContract : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        [Display(Name = "Cliente")]
        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        [Required(ErrorMessage = "O veículo é obrigatório.")]
        [Display(Name = "Veículo")]
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        public DateOnly? StartDate { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        [Display(Name = "Data de Fim")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "A quilometragem inicial é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quilometragem inicial não pode ser negativa.")]
        [Display(Name = "Quilometragem Inicial")]
        public int? InitialMileage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (StartDate.HasValue && StartDate.Value < today)
                yield return new ValidationResult(
                    "A data de início não pode ser anterior a hoje.", new[] { nameof(StartDate) });

            if (StartDate.HasValue && EndDate.HasValue && EndDate.Value <= StartDate.Value)
                yield return new ValidationResult(
                    "A data de fim tem de ser posterior à data de início.", new[] { nameof(EndDate) });
        }
    }
}
