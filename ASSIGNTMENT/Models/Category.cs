using System;
using System.Collections.Generic;

namespace ASSIGNTMENT.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
