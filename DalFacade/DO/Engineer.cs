namespace DO;
/// <summary>
/// An engineer entity represents an engineer with all its props.
/// </summary>
/// <param name="Id">Personal unique ID of engineer (as in national id card)</param>
/// <param name="Name">Private name of the engineer</param>
/// <param name="Email">Email address of an engineer</param>
/// <param name="level">The level of the engineer</param>
/// <param name="cost">Cost per hour of an engineer</param>
public record Engineer
(
    int Id,
    string Name,
    string Email,
    EngineerExperience Level,
    double Cost,
    bool Active = true
)
{
    public Engineer() : this(0, "", "", EngineerExperience.JUNIOR, 0) { }
}