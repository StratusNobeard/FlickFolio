using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Serija
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public string PocetnaGodina { get; set; } = null!;

    public string ZavrsnaGodina { get; set; } = null!;

    public string BrojSezona { get; set; } = null!;

    public int RedateljId { get; set; }

    public virtual Redatelj Redatelj { get; set; } = null!;

    public virtual ICollection<SerijaGlumac> SerijaGlumac { get; set; } = new List<SerijaGlumac>();

    public virtual ICollection<SerijaZanr> SerijaZanr { get; set; } = new List<SerijaZanr>();

    public virtual ICollection<Sezona> Sezone { get; set; } = new List<Sezona>();
}
