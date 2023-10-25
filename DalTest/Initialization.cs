namespace DalTest;
using DalApi;
using DO;
using System.Security.Cryptography;

public static class Initialization
{
    private static IEngineer? s_dalEngineer;
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand = new();

    private static IEngineer? dalEngineer;
    private static ITask? dalTask;
    private static IDependency? dalDependency;


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
            while (s_dalEngineer!.Read(_id) != null);
            string _name = detailsEngineer[i].Item1;
            string _email = detailsEngineer[i].Item2;
            EngineerExperience _level = (EngineerExperience)(_id % 3);
            double _cost;
            switch (_level) //????????????????????????????????? מה המחיר לשעה של כל אחד
            {
                case EngineerExperience.Expert:
                    _cost = 450;
                    break;
                case EngineerExperience.Junior:
                    _cost = 200;
                    break;
                case EngineerExperience.Rookie:
                    _cost = 100;
                    break;
                default:
                    _cost = 30;
                    break;
            }
            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dalEngineer!.Create(newEngineer);
        }
    }

    // Create 100 tasks
    private static void createTasks() //לא עשינו פומקציית אתחול
    {
        
    }

    // Create 250 dependencies
    private static void createDependencies() //לא עשינו פומקציית אתחול
    {
        for (int i = 0; i < 150; i++)
        {

        }
    }
    //?????????????????????????????????//, לא אמרו שמקבלים פרמטרים
    private static void Do(IEngineer? s_dalEngineer, ITask? s_dalTask, IDependency? s_dalDependency) //???????????????
    {
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        createDependencies();
        createEngineers();
        createTasks();
    }
}