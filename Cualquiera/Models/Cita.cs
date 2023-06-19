using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Cita
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public int? IdMedico { get; set; }

    public int? IdSecretario { get; set; }

    public int IdPaciente { get; set; }

    public virtual ICollection<HistorialMedico> HistorialMedicos { get; set; } = new List<HistorialMedico>();

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
