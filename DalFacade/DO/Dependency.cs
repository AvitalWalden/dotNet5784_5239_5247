﻿namespace DO;
/// <summary>
/// Dependency entity represents a dependency between tasks with all its props
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">ID number of a previous assignment</param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }
}