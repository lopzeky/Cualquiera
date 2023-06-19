using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Especialidade
{
    public int Id { get; set; }

    public int? IdMedicos { get; set; }

    public string Especialidad { get; set; } = null!;

    public string? EspecialidadOpc { get; set; }

    public string? EspecialidadOpc2 { get; set; }
}
