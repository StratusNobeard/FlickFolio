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

    public virtual ICollection<FilmGlumac> FilmGlumac { get; set; } = new List<FilmGlumac>();

    public virtual ICollection<FilmZanr> FilmZanr { get; set; } = new List<FilmZanr>();

    public virtual Redatelj Redatelj { get; set; } = null!;
}
