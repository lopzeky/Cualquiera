using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cualquiera.Models;

public partial class Medico
{
    public int Id { get; set; }

    /*[Required(ErrorMessage = "El campo Nombres es obligatorio.")]
    [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "El campo Nombres solo puede contener letras.")]*/
    public string? Nombres { get; set; } = null!;

    /*[Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
    [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "El campo Apellidos solo puede contener letras.")]*/

    public string? Apellidos { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    /*[Required(ErrorMessage = "El campo de Rut es obligatorio.")]
    [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "El campo de Rut debe tener los parametros necesarios.")]*/
    public string? Rut { get; set; } = null!;

    /*[Required(ErrorMessage = "El campo Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo Email no tiene un formato válido.")]*/
    public string? Email { get; set; } = null!;

    public bool Disponible { get; set; }

    public string? Password { get; set; } = null!;
}
