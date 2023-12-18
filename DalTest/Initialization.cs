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
        const int MIN_ID = 200000000;
        const int MAX_ID = 400000000;
        (string, string)[] detailsEngineer = {
           ("Ayala Tov","ayala@gmail.com"),
           ("Yael Choen","yael@gmail.com"),
           ("Miri Miler","miri@gmail.com"),
           ("Shira Sharon","shira@gmail.com"),
           ("Leha Kaz", "leha@gmail.com"),
           ("Chaya Levi", "chaya@gmail.com"),
           ("Shani Walder", "shani@gmail.com"),
           ("Rachel Kalazan", "rachel@gmail.com"),
           ("Hadasa Zehavi", "hadasa@gmail.com"),
           ("David Wal", "david@gmail.com"),
           ("Yedidia Merin", "yedidia@gmail.com"),
           ("yaakov Eler", "yaakov@gmail.com"),
           ("batya Boier", "batya@gmail.com"),
           ("Miriam Gros", "miriam@gmail.com"),
           ("Efrat Yadin", "efrat@gmail.com"),
           ("Tamar Bloyi", "tamar@gmail.com"),
           ("Chaim Bashari", "chaim@gmail.com"),
           ("Menachem Erlanger", "menachem@gmail.com"),
           ("Shifra Dayan", "shifra@gmail.com"),
           ("Tzvi Sofer", "tzvi@gmail.com"),
           ("Daniel Kolin", "daniel@gmail.com"),
           ("Nethanel Sami", "nethanel@gmail.com"),
           ("Sam Golin", "sam@gmail.com"),
           ("Avraham Sol", "avraham@gmail.com"),
           ("Dov Firshtain", "dov@gmail.com"),
           ("Meir golin", "meir@gmail.com"),
           ("Shimon Shalom", "shimon@gmail.com"),
           ("Ben Shir", "ben@gmail.com"),
           ("Avital Walden", "avital@gmail.com"),
           ("Michal Dagan", "michal@gmail.com"),
           ("Sara Drori", "sara@gmail.com"),
           ("Tamar segal", "tamar@gmail.com"),
           ("Esti Wingerten", "esti@gmail.com"),
           ("Avi Silver", "miri@gmail.com"),
           ("shifi Braverman", "avi@gmail.com"),
           ("Chaim Winbarger", "chaim@gmail.com"),
           ("Sari Drilman", "sari@gmail.com"),
           ("Ruchama Bricker", "ruchama@gmail.com"),
           ("Tali temstet", "tali@gmail.com"),
           ("Yossi tair", "yossi@gmail.com")};

        for (int i = 0; i < detailsEngineer.Length; i++)
        {
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer.Read(_id) != null);
            string _name = detailsEngineer[i].Item1;
            string _email = detailsEngineer[i].Item2;
            EngineerExperience _level = (EngineerExperience)(_id % Enum.GetNames<EngineerExperience>().Count());
            double _cost;
            switch (_level) 
            {
                case EngineerExperience.Beginner:
                    _cost = 150.00;
                    break;
                case EngineerExperience.AdvancedBeginner:
                    _cost = 200.35;
                    break;
                case EngineerExperience.Competent:
                    _cost = 300.50;
                    break;
                     case EngineerExperience.Proficient:
                     _cost = 350.00;
                    break;
                case EngineerExperience.Expert:
                    _cost = 450.00;
                    break;
                default:
                    _cost = 30;
                    break;
            }
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dal!.Engineer.Create(newEngineer);
        }
    }

    // Create 100 tasks
    private static void createTasks() 
    {
        (string, string)[] detailsTask = 
        {
            ("Determining the purpose and requirements of the building","Target and requirements"),
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