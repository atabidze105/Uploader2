using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace DataUploaderApp.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string? Photo1 { get; set; }

    public virtual ICollection<Article> IdArticls { get; set; } = new List<Article>();

    public Bitmap PhotoLink => new Bitmap($"Assets/{Photo1}");

}
