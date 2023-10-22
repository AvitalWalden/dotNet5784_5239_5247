namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Description"></param>
/// <param name="Alias"></param>
/// <param name="Milestone"></param>
/// <param name="CreatedAt"></param>
/// <param name="Start"></param>
/// <param name="ForecastDate"></param>
/// <param name="Deadline"></param>
/// <param name="Complete"></param>
/// <param name="ProductDescription"></param>
/// <param name="Remarks"></param>
/// <param name="Engineerld"></param>
/// <param name="CopmlexityLevel"></param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    bool Milestone = false,
    //DateTime CreatedAt = DateTime.Now,
    DateTime? Start = null,
    DateTime? ForecastDate = null,
    DateTime? Deadline = null,
    DateTime? Complete = null,
    string? ProductDescription = null,
    string? Remarks = null,
    int? Engineerld = null,
    EngineerExperience? CopmlexityLevel = null
);