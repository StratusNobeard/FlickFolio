using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class FilmZanr
{
    public int Id { get; set; }

    public int ZanrId { get; set; }

    public virtual Film IdNavigation { get; set; } = null!;

    public virtual Zanr Zanr { get; set; } = null!;
}
