using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class Serija
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public int PocetnaGodina { get; set; }

    public int ZavrsnaGodina { get; set; }

    public int BrojSezona { get; set; }

    public int RedateljId { get; set; }

    public virtual Redatelj Redatelj { get; set; } = null!;

    public virtual ICollection<Sezona> Sezonas { get; set; } = new List<Sezona>();
}
