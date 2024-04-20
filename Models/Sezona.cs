using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Sezona
{
    public int Id { get; set; }

    public int RedniBroj { get; set; }

    public int BrojEpizoda { get; set; }

    public int SerijaId { get; set; }

    public virtual Serija Serija { get; set; } = null!;
}
