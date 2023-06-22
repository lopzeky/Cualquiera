using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cualquiera.Models;
using System.Text.RegularExpressions;

namespace Cualquiera.Controllers
{
    
    public class AdministradorsController : Controller
    {
        private readonly ClinicaContext _context;

        public AdministradorsController(ClinicaContext context)
        {
            _context = context;
        }

        // GET: Administradors
        public async Task<IActionResult> Index(string buscar)
        {
            var admin = from Administrador in _context.Administradors select Administrador;
            //condicion 
            if (!String.IsNullOrEmpty(buscar))
            {
                admin = admin.Where(s => s.Usuario!.Contains(buscar));
            }
            return View(await admin.ToListAsync());
                          
        }

        // GET: Administradors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Administradors == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: Administradors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Usuario,Password,Rut")] Administrador administrador)
        {
            if (!SoloLetras(administrador.Usuario))
            {
                ModelState.AddModelError("Usuario", "El Usuario ingresado no es válido.");
            }
            if (!EsRutValido(administrador.Rut))
            {
                ModelState.AddModelError("Rut", "El Rut ingresado no es válido.");
            }
            if (!LargoPass(administrador.Password))
            {
                ModelState.AddModelError("Password","El largo debe ser entre 5 y 8");
            }
            if (ModelState.IsValid)
            {
                _context.Add(administrador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        public bool SoloLetras(string cadena)
        {
            //Regex regex = new Regex(@"[^a-zA-Z]");
            //if (regex.IsMatch(cadena))
            if (Regex.IsMatch(cadena, @"[^a-zA-Z]"))
            {
                return false;
            }
            return true;
        }

        public bool LargoPass(string x)
        {
            int y = x.Length;
            if (y >= 5 && y <= 8)
            {
                return true;
            }
            return false;
        }

        private bool EsRutValido(string rut)
        {
            rut = rut.Replace(".", "").Replace("-", "");
            if (!Regex.IsMatch(rut, @"^\d{7,8}[0-9Kk]$"))
            {
                return false;
            }
            char dv = rut[rut.Length - 1];
            string cuerpo = rut.Substring(0, rut.Length - 1);
            int suma = 0;
            int multiplicador = 2;
            for (int i = cuerpo.Length - 1; i >= 0; i--)
            {
                suma += int.Parse(cuerpo[i].ToString()) * multiplicador;
                multiplicador = multiplicador == 7 ? 2 : multiplicador + 1;
            }
            int resto = suma % 11;
            int verificador = 11 - resto;
            if (verificador == 10 && (dv == 'K' || dv == 'k'))
            {
                return true;
            }
            else if (verificador == 11 && dv == '0')
            {
                return true;
            }
            else if (verificador == int.Parse(dv.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Administradors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Administradors == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: Administradors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Password,Rut")] Administrador administrador)
        {
            if (id != administrador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorExists(administrador.Id))
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
            return View(administrador);
        }

        // GET: Administradors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Administradors == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: Administradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Administradors == null)
            {
                return Problem("Entity set 'ClinicaContext.Administradors'  is null.");
            }
            var administrador = await _context.Administradors.FindAsync(id);
            if (administrador != null)
            {
                _context.Administradors.Remove(administrador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministradorExists(int id)
        {
          return (_context.Administradors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
