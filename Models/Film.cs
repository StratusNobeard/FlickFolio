using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Film
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public string Trajanje { get; set; } = null!;

    public string Godina { get; set; } = null!;

    public int RedateljId { get; set; }

    public virtual FilmGlumci? FilmGlumci { get; set; }

    public virtual FilmZanr? FilmZanr { get; set; }

    public virtual Redatelj Redatelj { get; set; } = null!;

    public virtual SerijaGlumac? SerijaGlumac { get; set; }

    public virtual SerijaZanr? SerijaZanr { get; set; }
}
