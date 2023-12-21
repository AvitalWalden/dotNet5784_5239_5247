namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Security.Cryptography;

public static class Initialization
{
    //private static IEngineer? s_dalEngineer;
    //private static ITask? s_dalTask;
    //private static IDependency? s_dalDependency;
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    // Create 40 engineers
    private static void createEngineers()
    {
        (int, string, string, EngineerExperience, double)[] detailsEngineer = {
           (214385247, "Michal Walden","michal@gmail.com", (EngineerExperience)4, 450),
           (214385239, "Avital Choen","avital@gmail.com", (EngineerExperience)3, 399.99),
           (016438046, "David Tov","david@gmail.com", (EngineerExperience)2, 250),
           (344165165, "Ayala Dagan","ayala@gmail.com", (EngineerExperience)1, 200),
           (111111118, "Miri Silver","miri@gmail.com", (EngineerExperience)0, 100),
           (023995871, "Esty Shalom","esty@gmail.com", (EngineerExperience)4, 450),
           (214385242, "Sara Drori","sara@gmail.com", (EngineerExperience)3, 415),
           (214385247, "Yair Goldshtein","yair@gmail.com", (EngineerExperience)3, 400),
           (214385244, "Sari Drilman","sari@gmail.com", (EngineerExperience)2, 250),
           (214385243, "Shifra Dayan","shifra@gmail.com", (EngineerExperience)4, 199.90),
           (214258965, "Yossi Walden","sari@gmail.com", (EngineerExperience)2, 250),
        };

        for (int i = 0; i < detailsEngineer.Length; i++)
        {
            Engineer newEngineer = new(detailsEngineer[i].Item1, detailsEngineer[i].Item2, detailsEngineer[i].Item3, detailsEngineer[i].Item4, detailsEngineer[i].Item5);
            s_dal!.Engineer.Create(newEngineer);
        }
    }

    // Create 100 tasks
    private static void createTasks() 
    {
        (string, string)[] detailsTask = 
        {
            ("תכנון הבנייה על פי ייעוד ודרישות הבניין" ,"דרישות הבניין"),
            ("Purchase of land or the necessary property","Purchases"),
            ("Guaranteed funding","Funding"),
            ("Obtaining necessary permits for construction","Construction permits"),
            ("Meetings with an architect to create a work plan","Create a work plan"),
            ("Land clearing and grading","Preparing the website"),
            ("Digging the ground","Excavations"),
            ("Creating a marking outline","Marking outline"),
            ("The casting of the skeleton of the building or site","Castings"),
            ("Construction of exterior walls","Exterior walls"),
        };
        List<Engineer?> allEngineer = s_dal!.Engineer.ReadAll().ToList();

        for (int i = 0; i < detailsTask.Length; i++)
        {
            //Set other task properties as needed
            Task newTask = new(
                0,
                detailsTask[i].Item1,
                detailsTask[i].Item2,
                DateTime.Now,
                TimeSpan.Zero,
                false, // Milestone 
                DateTime.Now.AddDays(i * 5).AddDays(i + 1), // Start Date 
                DateTime.Now.AddDays(i * 5).AddDays(i + 2), // ScheduledDate date
                DateTime.Now.AddDays(i * 5).AddDays(i + 3), // DeadLine date
                DateTime.Now.AddDays(i * 5).AddDays(i + 4), // Complete date
                "Product description for " + detailsTask[i].Item1,
                "Remarks for " + detailsTask[i].Item2,
                allEngineer[i]!.Id,
                allEngineer[i]!.Level
            );
            s_dal!.Task.Create(newTask);
        }
    }

    // Create 250 dependencies
    private static void createDependencies()
    {
        Dependency dependency = new Dependency(0, 0,1);
        s_dal!.Dependency.Create(dependency);
        Dependency dependency1 = new Dependency(0, 1, 2);
        s_dal!.Dependency.Create(dependency1);
        Dependency dependency2 = new Dependency(0, 3, 5);
        s_dal!.Dependency.Create(dependency2);
        Dependency dependency3 = new Dependency(0, 4, 6);
        s_dal!.Dependency.Create(dependency3);
    }
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4
        createEngineers();
        createTasks();
        createDependencies();
    }
}