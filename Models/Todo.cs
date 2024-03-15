using System;
using System.Collections.Generic;

namespace TodoListAPP.Models;

public partial class Todo
{
    public int TodoId { get; set; }

    public int? UserId { get; set; }

    public int Priority { get; set; }

    public string Task { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
