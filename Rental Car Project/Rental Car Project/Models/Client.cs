using System.ComponentModel.DataAnnotations;

namespace Rental_Car_Project.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; } = string.Empty;
        
        [Required, StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(20)]
        [Display(Name = "Carta de Condução")]
        public string DriverLicenseNumber { get; set; } = string.Empty;

        [Required, StringLength(15)]
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
    }
}
