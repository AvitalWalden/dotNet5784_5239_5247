namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Email"></param>
/// <param name="level"></param>
/// <param name="cost"></param>
public record Engineer
(
    int Id,
    string Name,
    string Email,
    EngineerExperience level,
    double cost
);