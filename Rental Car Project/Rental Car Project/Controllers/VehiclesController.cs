using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental_Car_Project.Data;
using Rental_Car_Project.Models;
using Rental_Car_Project.ViewModels;

namespace Rental_Car_Project.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Cria uma lista de veículos com a informação se estão alugados ou não
            var vehicles = await _context.Vehicles
                .OrderBy(v => v.Brand)
                .ThenBy(v => v.Model)
                .Select(v => new VehicleListItemViewModel
                {
                    Vehicle = v,
                    IsRented = v.RentalContracts.Any(r => r.StartDate <= today && r.EndDate >= today)
                })
                .ToListAsync();

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,LicensePlate,ManufactureYear,FuelType")] Vehicle vehicle)
        {
            // Normaliza a matrícula: remove espaços em branco e converte para maiúsculas
            vehicle.LicensePlate = vehicle.LicensePlate?.Trim().ToUpper() ?? string.Empty;

            // Regra de negócio: a matrícula tem de ser única
            if (await _context.Vehicles.AnyAsync(v => v.LicensePlate == vehicle.LicensePlate))
            {
                ModelState.AddModelError(nameof(Vehicle.LicensePlate), "Já existe um veículo com esta matrícula.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,LicensePlate,ManufactureYear,FuelType")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            // Uniformiza a matrícula (sem espaços e em maiúsculas) antes de validar e gravar
            vehicle.LicensePlate = vehicle.LicensePlate?.Trim().ToUpper() ?? string.Empty;

            // A matrícula tem de ser única (excluindo o próprio veículo)
            if (await _context.Vehicles.AnyAsync(v => v.LicensePlate == vehicle.LicensePlate && v.Id != vehicle.Id))
            {
                ModelState.AddModelError(nameof(Vehicle.LicensePlate), "Já existe um veículo com esta matrícula.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Não se elimina um veículo com contratos, para preservar o histórico
            if (await _context.RentalContracts.AnyAsync(rc => rc.VehicleId == id))
            {
                ModelState.AddModelError(string.Empty, "Não é possível eliminar este veículo porque existem contratos de aluguer associados.");
                return View(vehicle);
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
