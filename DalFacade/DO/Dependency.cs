namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="DependentTask"></param>
/// <param name="DependsOnTask"></param>
public record Dependency
(
    int Id,
    int? DependentTask,
    int? DependsOnTask
);