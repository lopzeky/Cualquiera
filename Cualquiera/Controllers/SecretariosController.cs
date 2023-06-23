using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cualquiera.Models;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cualquiera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SecretariosController : Controller
    {
        private readonly ClinicaContext _context;

        public SecretariosController(ClinicaContext context)
        {
            _context = context;
        }

        // GET: Secretarios
        public async Task<IActionResult> Index()
        {
              return _context.Secretarios != null ? 
                          View(await _context.Secretarios.ToListAsync()) :
                          Problem("Entity set 'ClinicaContext.Secretarios'  is null.");
        }

        // GET: Secretarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Secretarios == null)
            {
                return NotFound();
            }

            var secretario = await _context.Secretarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretario == null)
            {
                return NotFound();
            }

            return View(secretario);
        }

        // GET: Secretarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Secretarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,FechaNacimiento,Rut,Email,Password")] Secretario secretario, Contra contras)
        {
            if(!SoloLetras(secretario.Nombres))
            {
                ModelState.AddModelError("Nombres", "El Nombre ingresado no es válido.");
            }
            if (!SoloLetras(secretario.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "El Apellido ingresado no es válido.");
            }
            if (!EsRutValido(secretario.Rut))
            {
                ModelState.AddModelError("Rut", "El Rut ingresado no es válido.");
            }
            if (!LargoPass(secretario.Password))
            {
                ModelState.AddModelError("Password", "El largo debe ser entre 5 y 8");
            }
            if (!SoloEmail(secretario.Email))
            {
                ModelState.AddModelError("Email", "El Email ingresado no es válido.");
            }
            if (!secretario.Password.Equals(contras.contra))
            {
                ModelState.AddModelError("Password", "La contraseña no coincide");
            }
            if (ModelState.IsValid)
            {
                _context.Add(secretario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(secretario);
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
        /*[HttpPost]
        public IActionResult ValidarRut(string rut)
        {
            bool isValid = EsRutValido(rut);

            return Json(new { isValid });
        }*/

        // GET: Secretarios/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Secretarios == null)
            {
                return NotFound();
            }
            var secretario = await _context.Secretarios.FindAsync(id);
            if (secretario == null)
            {
                return NotFound();
            }
            return View(secretario);
        }
        // POST: Secretarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,FechaNacimiento,Rut,Email,Password")] Secretario secretario, Contra contras)
        {
            if (id != secretario.Id && !EsRutValido(secretario.Rut))
            {
                return NotFound();
            }
            if (!SoloLetras(secretario.Nombres))
            {
                ModelState.AddModelError("Nombres", "El Nombre ingresado no es válido.");
            }
            if (!SoloLetras(secretario.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "El Apellido ingresado no es válido.");
            }
            if (!EsRutValido(secretario.Rut))
            {
                ModelState.AddModelError("Rut", "El Rut ingresado no es válido.");
            }
            if (!SoloEmail(secretario.Email))
            {
                ModelState.AddModelError("Email", "El Email ingresado no es válido.");
            }
            if (!secretario.Password.Equals(contras.contra))
            {
                ModelState.AddModelError("Password", "La contraseña no coincide");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secretario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretarioExists(secretario.Id))
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
            return View(secretario);
        }

        // GET: Secretarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Secretarios == null)
            {
                return NotFound();
            }

            var secretario = await _context.Secretarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretario == null)
            {
                return NotFound();
            }

            return View(secretario);
        }

        // POST: Secretarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Secretarios == null)
            {
                return Problem("Entity set 'ClinicaContext.Secretarios'  is null.");
            }
            var secretario = await _context.Secretarios.FindAsync(id);
            if (secretario != null)
            {
                _context.Secretarios.Remove(secretario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecretarioExists(int id)
        {
          return (_context.Secretarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
