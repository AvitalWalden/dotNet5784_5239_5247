﻿namespace BO;

public class MilestoneInTask
{
    public int Id { get; init; }
    public required string Description { get; set; } // דרש
    public required string Alias { get; set; } // דרש
    public Status? Status { get; set; } = null;
    public double? CompletionPercentage { get; set; } = null;
}
