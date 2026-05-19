using System;
using System.Collections.Generic;

namespace ASSIGNTMENT.Models;

public partial class ComboDetail
{
    public int Id { get; set; }

    public int? ComboId { get; set; }

    public int? FoodId { get; set; }

    public int? Quantity { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Food? Food { get; set; }
}
