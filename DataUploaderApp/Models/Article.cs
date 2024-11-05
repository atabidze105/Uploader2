using System;
using System.Collections.Generic;
using System.Linq;

namespace DataUploaderApp.Models;

public partial class Article()
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Video { get; set; }

    public string? Music { get; set; }

    public int IdCathegorie { get; set; } 

    public DateOnly AddDate { get; set; }

    public virtual Cathegory IdCathegorieNavigation { get; set; } = null!;

    public virtual ICollection<Photo> IdPhotos { get; set; } = new List<Photo>();

    public string Date => AddDate.ToString();

}
