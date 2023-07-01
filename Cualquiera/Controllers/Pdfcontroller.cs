using Cualquiera.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Cualquiera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class Pdfcontroller : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public List<Administrador> ObteberDatosdeLaBaseDeDatos(List<Medico> data)
        {
            List<Administrador> datos = new List<Administrador>();
            using (var dbContext = new ClinicaContext())
            {
                datos = dbContext.Administradors.ToList();
            }
            return datos;
        }
        [HttpPost]
        [Route("/Pdf/RecibirDatos")]
        public IActionResult RecibirDatos(string nombre, string apellidos, string fechanac, string rut, string email)
        {
            List<string> datitos = new List<string>();
            datitos.Add(nombre);
            datitos.Add(apellidos);
            datitos.Add(fechanac);
            datitos.Add(rut);
            datitos.Add(email);

            return Ok();
        }

        [HttpPost]
        [Route("/Controllers/Pdfcontroller/GeneratePdf")]
        public async Task<IActionResult> GeneratePdf(string datos)
        {

            var lista = JsonConvert.DeserializeObject<string[][]>(datos);

            // Crear el documento y establecer el tamaño de la página
            Document documento = new Document(PageSize.A4, 50, 50, 25, 25);

            
            var stream = new MemoryStream();
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            // Crear el escritor de PDF y adjuntarlo al flujo de datos
            PdfWriter writer = PdfWriter.GetInstance(documento, stream);
            writer.CloseStream = false;
                documento.Open();

                // Fuente y estilo para el título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph("Lista de Usuarios", titleFont);
                title.Alignment = Element.ALIGN_CENTER;

                // Agregar el título al documento
                documento.Add(title);


                // Fuente y estilo para los datos de usuario
                var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            documento.Add(new Paragraph("Nombre__Apellidos___Fecha____________Rut_____________Email"));
            for (int i = 0; i < lista.Length; i++)
            {
                string nom = lista[i][0].ToString().Trim();
                string appell = lista[i][1].ToString().Trim();
                string fech = lista[i][2].ToString().Trim();
                List<char> caracteres = fech.ToList();
                caracteres.RemoveRange(caracteres.Count - 7, 7);
                string fechass = new string(caracteres.ToArray());
                string ru = lista[i][3].ToString().Trim();
                string em = lista[i][4].ToString().Trim();

                documento.Add(new Paragraph($"{nom}__{appell}___{fechass}____{ru}_____{em}"));
            }

            documento.Close();
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

            //}

            var response = HttpContext.Response;
            response.ContentType = "application/pdf";
            response.Headers.Add("Content-Disposition", "attachment; filename=archivo.pdf");
            var file = new FileStreamResult(stream, "application/pdf");

            file.FileDownloadName = "Reporte.pdf";

            return file;
            /*using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                await fileStream.CopyToAsync(response.Body);
            }*/

           
        }
    }
}
