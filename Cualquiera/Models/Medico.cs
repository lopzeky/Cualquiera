using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Medico
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Rut { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Disponible { get; set; }

    public string Password { get; set; } = null!;
}
