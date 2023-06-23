using System;
using System.Collections.Generic;

namespace Cualquiera.Models;

public partial class Administrador
{
    internal string pwd2;

    public int Id { get; set; }

    public string? Usuario { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Rut { get; set; } = null!;
}
