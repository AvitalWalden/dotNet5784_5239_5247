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

    // Create 11 engineers
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

    // Create 20 tasks
    private static void createTasks() 
    {
        (string, string, string)[] detailsTask =
        {
            ("תכנון הבנייה על פי ייעוד ודרישות הבנין" ,"ארגון ותכנון הבניה", "תכנון הבניה הושלם"), //task 0
            ("הבטחת מימון פרויקט הבניה","מימון","מימון או מימון התחלתי לפרויקט הבנייה"), //task 1
            ("רכישת הקרקע","רכישת הנכס", "קרקע בבעלות החברה לקראת תחילת עבודות בניה במקום"), //task 2
            ("בקשת אישורים נחוצים לצורך הבנייה","אישורים לפרויקט הבנייה","קבלת האישורים הנחוצים לבניה"), //task 3
            ("פגישות עם האדריכל או צוות העיצוב ליצירת תוכנית עבודה","עיצוב","תוכנית עיצוב מוכנה"), //task 4
            ("התחלת הכנת האתר על ידי פינוי המקום","פינוי האתר","אתר הבניה מפונה מעצמים"), //task 5
            ("דירוג הקרקע והכנתה לבניה","דירוג","הקרקע מוכנה לבניה"), //task 6
            ("חפירת הקרקע","חפירות","הקרקע מוכנה לבניה"), //task 7
            ("יצירת מתווה סימון","מדידות","הקרקע מסומנת לבניה"), //task 8
            ("יציקת השלד של המבנה","יציקות","השלד מוכן"), //task 9
            ("בנית קירות חוץ","קירות בנין חיצוניות","קירות בנין חיצוניות בנויות"), //task 10
            ("בנית קירות פנים","קירות בנין פנימיות","קירות בנין פנימיות בנויות"), //task 11
            ("בנית מערכות בנין","מערכות בנין","הבנין יהיה עם מערכות חשמל ואינסטלציה"), //task 12
            ("טיוח הקירות","טיוח קירות","קירות מוכנות לצביעה"), //task 13
            ("ריצוף הבנין והדירות","ריצוף","בניין ודירות מרוצפות"), //task 14
            ("התקנת פתחים חלונות ודלתות","התקנת הפתחים","הבנין חסום מפתחים"), //task 15
            ("התקנת כלי עזר נחוצים","כלי עזר לבנין","הבנין בעל מים חשמל וגז"), //task 16
            ("בדיקות כדי להבטיח עמידה בקודי בניה ובתקנות","בדיקות","הבנין עומד בתקן"), //task 17
            ("נקיון יסוד של האתר בניה","נקיון אתר הבנייה","אתר הבניה נקי אין פסולת בנייה"), //task 18
            ("נקיון הבנין מהבניה","נקיון הבנין והדירות","הבנין נקי"), //task 19
            ("יפוי האתר על ידי גינון","גינון","האתר יפה ומושקע חיצונית בעזרת גינון ונקיון"), //task 20
            ("השגת תעודת איכלוס לאחר עברת כל הבדיקות","טופס 4","הבנין מוכן לאכלוס"), //task 21
            ("מסירת הבנין והדירות לבעלים","מסירת מפתחות","הדירות בידי הבעלים"), //task 22
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
                detailsTask[i].Item3,
                "Remarks for " + detailsTask[i].Item2,
                allEngineer[i]!.Id,
                allEngineer[i]!.Level
            );
            s_dal!.Task.Create(newTask);
        }
    }

    // Create 40 dependencies
    private static void createDependencies()
    {
        (int, int)[] dependencies = {
            (1 ,0), (2 ,0), (3 ,0), (4 ,3), (4 ,0), (4 ,1), (4 ,2), (4 ,3), (5 ,4), (6 ,5),
            (7 ,6), (7 ,5), (8 ,7), (9 ,8), (9 ,7), (16 ,0), (17 ,0), (18 ,0), (19 ,0), (31 ,0),
            (20 ,0), (21 ,0), (22 ,0), (23 ,0), (24 ,0), (25 ,0), (26 ,0), (27 ,0), (31 ,0), (31 ,0),
            (28 ,0), (29 ,0), (30 ,0), (31 ,0), (31 ,0), (31 ,0), (31 ,0), (31 ,0), (31 ,0) , (31 ,0)
        };

        for (int i = 0; i < dependencies.Length; i++)
        {
            Dependency dependency = new Dependency(0, dependencies[i].Item1, dependencies[i].Item2);
            s_dal!.Dependency.Create(dependency);

        }
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