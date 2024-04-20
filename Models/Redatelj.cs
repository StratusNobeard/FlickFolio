using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Redatelj
{
    public int Id { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public virtual ICollection<Film> Filmovi { get; set; } = new List<Film>();

    public virtual ICollection<Serija> Serije { get; set; } = new List<Serija>();
}
