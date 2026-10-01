using Rental_Car_Project.Models;

namespace Rental_Car_Project.ViewModels
{
    // Esta classe representa um item na lista de veículos, incluindo o veículo em si e se está alugado ou não.
    public class VehicleListItemViewModel
    {
        public Vehicle Vehicle { get; set; } = null!;
        public bool IsRented { get; set; }
    }
}
