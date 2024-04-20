using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class FilmGlumac
{
    public int Id { get; set; }

    public int FilmId { get; set; }

    public int GlumacId { get; set; }

    public virtual Film Film { get; set; } = null!;

    public virtual Glumac Glumac { get; set; } = null!;
}
