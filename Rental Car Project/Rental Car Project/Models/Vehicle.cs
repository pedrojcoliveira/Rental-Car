using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;

namespace Rental_Car_Project.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Marca")]
        public string Brand { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [Display(Name = "Modelo")]
        public string Model { get; set; } = string.Empty;

        [Required, StringLength(8)]
        [Display(Name = "Matrícula")]
        public string LicensePlate { get; set; } = string.Empty;

        [Display(Name = "Ano de Fabrico" )]
        public int ManufactureYear { get; set; }

        [Display(Name = "Tipo de Combustível")]
        public FuelType FuelType { get; set; }

        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
    }
}
