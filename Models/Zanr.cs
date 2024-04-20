using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Zanr
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<FilmZanr> FilmZanr { get; set; } = new List<FilmZanr>();

    public virtual ICollection<SerijaZanr> SerijaZanr { get; set; } = new List<SerijaZanr>();
}
