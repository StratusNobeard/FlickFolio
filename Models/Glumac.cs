using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Glumac
{
    public int Id { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public virtual ICollection<FilmGlumac> FilmGlumac { get; set; } = new List<FilmGlumac>();

    public virtual ICollection<SerijaGlumac> SerijaGlumac { get; set; } = new List<SerijaGlumac>();
}
