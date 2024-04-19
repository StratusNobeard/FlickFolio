using System;
using System.Collections.Generic;

namespace FlickFolio.Models;

public partial class SerijaGlumac
{
    public int Id { get; set; }

    public int GlumacId { get; set; }

    public virtual Glumac Glumac { get; set; } = null!;

    public virtual Film IdNavigation { get; set; } = null!;
}
