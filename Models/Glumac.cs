using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Glumac
{
    public int Id { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public virtual ICollection<FilmGlumci> FilmGlumcis { get; set; } = new List<FilmGlumci>();

    public virtual ICollection<SerijaGlumac> SerijaGlumacs { get; set; } = new List<SerijaGlumac>();
}
