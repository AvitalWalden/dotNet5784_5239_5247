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
    EngineerExperience level,
    double cost
)
{
    public Engineer() : this(0, "", "", EngineerExperience.JUNIOR, 0) { }

    //public Engineer(int Id, string Name, string Email, EngineerExperience level, double cost)
    //{
    //    this.Id = Id;
    //    this.Name = Name;
    //    this.Email = Email;
    //    this.level = level;
    //    this.cost = cost;
    //}
}