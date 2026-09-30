using System.ComponentModel.DataAnnotations;

namespace Rental_Car_Project.Models
{
    public class RentalContract
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        [Display(Name = "Veículo")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        [Display(Name = "Data de Início")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Quilometragem Inicial")]
        public int InitialMileage { get; set; }
    }
}
