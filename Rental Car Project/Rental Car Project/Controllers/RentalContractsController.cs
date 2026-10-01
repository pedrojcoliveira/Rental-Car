using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental_Car_Project.Data;
using Rental_Car_Project.Models;

namespace Rental_Car_Project.Controllers
{
    public class RentalContractsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalContractsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RentalContracts
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.RentalContracts
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return View(contracts);
        }

        // GET: RentalContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.RentalContracts
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: RentalContracts/Create
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: RentalContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,VehicleId,StartDate,EndDate,InitialMileage")] RentalContract contract)
        {
            if (ModelState.IsValid && await HasOverlappingContractAsync(contract))
            {
                ModelState.AddModelError(string.Empty, "Já existe um contrato de aluguer para este veículo que se sobrepõe às datas fornecidas.");
            }

            if ((ModelState.IsValid))
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Se o modelo não for válido, ou se houver um contrato sobreposto, preenche os dropdowns novamente para que o utilizador possa corrigir os erros.
            PopulateDropDowns(contract.ClientId, contract.VehicleId);
            return View(contract);
        }


        // Dois contratos de aluguer para o mesmo veículo não podem ter datas que se sobreponham. Esta função verifica se existe algum contrato de aluguer que se sobreponha ao contrato fornecido.
        private Task<bool> HasOverlappingContractAsync(RentalContract contract)
        {
            return _context.RentalContracts
                .AnyAsync(c => c.VehicleId == contract.VehicleId &&
                               c.Id != contract.Id &&
                               c.StartDate < contract.EndDate &&
                               contract.StartDate < c.EndDate);
        }


        // Preenche os dropdowns de clientes e veículos com os dados da base de dados, e seleciona o cliente e veículo fornecidos (se houver).
        private void PopulateDropDowns(int? selectedClientId = null, int? selectedVehicleId = null)
        {
            var clients = _context.Clients.Select(c => new { c.Id, Text = c.Name + " (" + c.Email + ")" }).ToList();
            var vehicles = _context.Vehicles.Select(v => new { v.Id, Text = v.Brand + " " + v.Model + " (" + v.LicensePlate + ")" }).ToList();

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", selectedClientId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Brand", selectedVehicleId);

        }
    }
}


