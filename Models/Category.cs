using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoListAPP.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "The Name of Category is required")]
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
