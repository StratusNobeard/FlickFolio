using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class SerijaZanr
{
    public int Id { get; set; }

    public int SerijaId { get; set; }

    public int ZanrId { get; set; }

    public virtual Serija Serija { get; set; } = null!;

    public virtual Zanr Zanr { get; set; } = null!;
}
