﻿namespace DO;
/// <summary>
/// Dependency entity represents a task with all its props
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="Description">Description of the task</param>
/// <param name="Alias">nickname of the task</param>
/// <param name="Milestone">Milestone (Boolean)</param>
/// <param name="Start">Creation date of the task</param>
/// <param name="ScheduledDate">Start date of the task</param>
/// <param name="ForecastDate">Estimated date of completion the task</param>
/// <param name="Deadline">Final date for completion the task</param>
/// <param name="Complete">Task completion date</param>
/// <param name="ProductDescription">Product (a string describing the product)</param>
/// <param name="Remarks">Notes of the task</param>
/// <param name="Engineerld">The ID of the engineer assigned to the task</param>
/// <param name="CopmlexityLevel">Difficulty level of the task</param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    bool Milestone = false,
    DateTime? CreatedAt = null,
    DateTime? Start = null,
    DateTime? ScheduledDate = null,
    DateTime? ForecastDate = null,
    DateTime? Deadline = null,
    DateTime? Complete = null,
    string? ProductDescription = null,
    string? Remarks = null,
    int? Engineerld = null,
    EngineerExperience? CopmlexityLevel = null
);