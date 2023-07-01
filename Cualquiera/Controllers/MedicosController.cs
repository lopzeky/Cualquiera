using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cualquiera.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cualquiera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MedicosController : Controller
    {
        private readonly ClinicaContext _context;

        public MedicosController(ClinicaContext context)
        {
            _context = context;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            return _context.Medicos != null ?
            View(await _context.Medicos.ToListAsync()) :
            Problem("Entity set 'ClinicaContext.Medicos'  is null.");
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,FechaNacimiento,Rut,Email,Disponible,Password")] Medico medicos, Contra contras)
        {
            if (!SoloLetras(medicos.Nombres))
            {
                ModelState.AddModelError("Nombres", "El Nombre ingresado no es válido.");
            }
            if (!SoloLetras(medicos.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "El Apellido ingresado no es válido.");
            }
            if (!EsRutValido(medicos.Rut))
            {
                ModelState.AddModelError("Rut", "El Rut ingresado no es válido.");
            }
            if (!SoloEmail(medicos.Email))
            {
                ModelState.AddModelError("Email", "El Email ingresado no es válido.");
            }
            if (!LargoPass(medicos.Password))
            {
                ModelState.AddModelError("Password", "El largo debe ser entre 5 y 8");
            }
            if (!medicos.Password.Equals(contras.contra))
            {
                ModelState.AddModelError("Password", "La contraseña no coincide");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(medicos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicos);
        }
        public bool SoloLetras(string cadena)
        {
            //Regex regex = new Regex(@"[^a-zA-Z]");
            //if (regex.IsMatch(cadena))
            if (Regex.IsMatch(cadena, @"[^a-zA-Z-\p{L}]"))
            {
                return false;
            }
            return true;
        }
        public bool LargoPass(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                int y = x.Length;
                if (y >= 5 && y <= 8)
                {
                    return true;
                }
                return false;
            }
            return false;

        }
        public bool SoloEmail(string email)
        {
            string emailPatron = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Match match = Regex.Match(email, emailPatron);
            return match.Success;
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

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,FechaNacimiento,Rut,Email,Disponible,Password")] Medico medico, Contra contras)
        {
            if (id != medico.Id)
            {
                return NotFound();
            }
            
            if (!SoloLetras(medico.Nombres))
            {
                ModelState.AddModelError("Nombres", "El Nombre ingresado no es válido.");
            }
            if (!SoloLetras(medico.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "El Apellido ingresado no es válido.");
            }
            if (!EsRutValido(medico.Rut))
            {
                ModelState.AddModelError("Rut", "El Rut ingresado no es válido.");
            }
            if (!SoloEmail(medico.Email))
            {
                ModelState.AddModelError("Email", "El Email ingresado no es válido.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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
            return View(medico);
        }
        

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicos == null)
            {
                return Problem("Entity set 'ClinicaContext.Medicos'  is null.");
            }
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
          return (_context.Medicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        public IActionResult name([RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "El campo solo puede contener texto.")] string campoTexto)
        {
            if (ModelState.IsValid)
            {
                // El campo de texto es válido, realizar acciones adicionales aquí
                // ...
                return RedirectToAction("OtraAccion");
            }

            // El campo de texto no es válido, volver a la vista con los errores de validación
            return View();
        }
    }
}
