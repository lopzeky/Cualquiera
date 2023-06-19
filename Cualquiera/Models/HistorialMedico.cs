using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class HistorialMedico
{
    public int Id { get; set; }

    public string Diagnostico { get; set; } = null!;

    public string? Alergias { get; set; }

    public int IdCita { get; set; }

    public virtual Cita IdCitaNavigation { get; set; } = null!;
}
