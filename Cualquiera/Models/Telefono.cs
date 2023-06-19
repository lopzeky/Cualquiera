using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Telefono
{
    public int Id { get; set; }

    public int? IdMedicos { get; set; }

    public int NumeroTel { get; set; }

    public int? NumeroTelOp { get; set; }
}
