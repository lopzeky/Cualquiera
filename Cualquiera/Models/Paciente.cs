using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Paciente
{
    public int Id { get; set; }

    public string? Nombres { get; set; } = null!;

    public string? Apellidos { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string? Rut { get; set; } = null!;

    public string? Email { get; set; }

    public int Telefono { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
