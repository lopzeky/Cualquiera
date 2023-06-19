using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Cualquiera.Models;
using Microsoft.EntityFrameworkCore;

namespace Cualquiera.Controllers
{
    public class Pdfcontroller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public List<Administrador> ObteberDatosdeLaBaseDeDatos()
        {
            List<Administrador> datos = new List<Administrador>();
            using (var dbContext = new ClinicaContext())
            {
                datos = dbContext.Administradors.ToList();
            }
            return datos;
        }
        public async Task<IActionResult> GeneratePdf()
        {
            // Crear el documento y establecer el tamaño de la página
            Document documento = new Document(PageSize.A4, 50, 50, 25, 25);

            var filePath = "C:/Users/lopez/OneDrive/Escritorio/archivo.pdf";

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Crear el escritor de PDF y adjuntarlo al flujo de datos
                PdfWriter writer = PdfWriter.GetInstance(documento, stream);
                documento.Open();

                // Fuente y estilo para el título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph("Lista de Usuarios", titleFont);
                title.Alignment = Element.ALIGN_CENTER;

                // Agregar el título al documento
                documento.Add(title);

                // Fuente y estilo para los datos de usuario
                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var userData = ObteberDatosdeLaBaseDeDatos();

                // Agregar los datos de usuario al documento
                foreach (var datos in userData)
                {
                    documento.Add(new Paragraph($"Usuario: {datos.Usuario}", dataFont));
                    documento.Add(new Paragraph($"Rut: {datos.Rut}", dataFont));
                    documento.Add(new Paragraph("")); // Espacio en blanco entre cada usuario
                }

                documento.Close();
            }

            var response = HttpContext.Response;
            response.ContentType = "application/pdf";
            response.Headers.Add("Content-Disposition", "attachment; filename=archivo.pdf");

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                await fileStream.CopyToAsync(response.Body);
            }

            return new EmptyResult();
        }

    }
}
