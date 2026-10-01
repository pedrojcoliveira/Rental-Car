using System.ComponentModel.DataAnnotations;

namespace Rental_Car_Project.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email não tem um formato válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número da carta de condução é obrigatório.")]
        [StringLength(20, ErrorMessage = "O número da carta não pode ter mais de 20 caracteres.")]
        [Display(Name = "Carta de Condução")]
        public string DriverLicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^[29]\d{8}$",
            ErrorMessage = "O telefone deve ter 9 dígitos (apenas números) e começar por 2 ou 9.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
    }
}
