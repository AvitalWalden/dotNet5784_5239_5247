using System.Collections.Generic;

namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// Configuration department, which produces automatic running numbers for us
    /// </summary>
    internal static class Config
    {
         
        internal const int startTaskId = 0;// A constant numeric field (const) that receives an initial value for a running number - startTaskId: the smallest identifying number.
        private static int nextTaskId = startTaskId;// A static numeric field that will receive as an initial value the previous fixed field.
        internal static int NextTaskId { get => nextTaskId++; }//A property with get that advances the private field automatically, by a number greater than the previous one by 1.


        internal const int startDependencyId = 0;//A constant numeric field (const) that receives an initial value for a running number - startDependencyId: the smallest identifying number.
        private static int nextDependencyId = startDependencyId;// A static numeric field that will receive as an initial value the previous fixed field.
        internal static int NextDependencyId { get => nextDependencyId++; }//A property with get that advances the private field automatically, by a number greater than the previous one by 1.
        internal static DateTime endDateProject = new DateTime(2024, 1, 1);
        internal static DateTime startDateProject = new DateTime(2024, 1, 7);

    }

    internal static List<DO.Task?> Tasks { get; } = new();//Constructing a list of a task entity.
    internal static List<DO.Engineer?> Engineers { get; } = new();//Building a list of an engineer entity.
    internal static List<DO.Dependency?> Dependencies { get; } = new();//Building a list of entity dependencies.

}