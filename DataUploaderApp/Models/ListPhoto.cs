using System;
using System.Collections.Generic;

namespace DataUploaderApp.Models;

public partial class ListPhoto
{
    public int IdArticl { get; set; }

    public int IdPhoto { get; set; }

    public virtual Article IdArticlNavigation { get; set; } = null!;

    public virtual Photo IdPhotoNavigation { get; set; } = null!;
}
