using System;
using System.Collections.Generic;

namespace forum.Models;

public partial class Article
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime CreationTime { get; set; }

    public Guid CreationUserId { get; set; }
}
