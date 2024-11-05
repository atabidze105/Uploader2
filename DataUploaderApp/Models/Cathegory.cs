using System;
using System.Collections.Generic;

namespace DataUploaderApp.Models;

public partial class Cathegory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
