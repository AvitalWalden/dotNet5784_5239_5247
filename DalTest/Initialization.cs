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
           (214385247, "Michal Walden","michal@gmail.com", (EngineerExperience)4, 450), //1
           (214385239, "Avital Choen","avital@gmail.com", (EngineerExperience)3, 399.99), //2
           (016438046, "David Tov","david@gmail.com", (EngineerExperience)2, 250), //3
           (344165165, "Ayala Dagan","ayala@gmail.com", (EngineerExperience)1, 200), //4
           (111111118, "Miri Silver","miri@gmail.com", (EngineerExperience)0, 100), //5
           (023995871, "Esty Shalom","esty@gmail.com", (EngineerExperience)4, 450), //6
           (214385242, "Sara Drori","sara@gmail.com", (EngineerExperience)3, 415), //7
           (214375247, "Yair Goldshtein","yair@gmail.com", (EngineerExperience)3, 400), //8
           (214385244, "Sari Drilman","sari@gmail.com", (EngineerExperience)2, 250), //9
           (214385243, "Shifra Dayan","shifra@gmail.com", (EngineerExperience)4, 199.90), //10
           (214258965, "Yossi Walden","yossi@gmail.com", (EngineerExperience)2, 250), //11
           (214785269, "Motti Dan","Motti@gmail.com", (EngineerExperience)2, 250), //12
           (320569824, "Yonni Rakov","Yonni@gmail.com", (EngineerExperience)3, 300.50), //13
           (214385269, "Dan Zilbershtoin","dan@gmail.com", (EngineerExperience)4, 500), //14
           (214385236, "Ruti Ben-Daviv","ruti@gmail.com", (EngineerExperience)4, 500), //15
           (314385242, "Ayala Drori","ayala@gmail.com", (EngineerExperience)3, 500),//16
           (314385247, "Yair Dor","yair@gmail.com", (EngineerExperience)3, 450),//17
           (314385244, "Yael Drilman","yael@gmail.com", (EngineerExperience)2, 220),//18
           (314385243, "Shira Shalom","Shira@gmail.com", (EngineerExperience)4, 189.90),//19
           (314258965, "Chaim Ben-Baruch ","Chaim@gmail.com", (EngineerExperience)2, 230),//20
           (324385243, "Shira Sal","Shira@gmail.com", (EngineerExperience)1, 170.90),//21
           (324258965, "Chaim Sofer ","Chaim@gmail.com", (EngineerExperience)2, 230),//22
           (322258965, "Tali Rubin","Chaim@gmail.com", (EngineerExperience)2, 230),//23
        };

        foreach (var engineerDetails in detailsEngineer)
        {
            Engineer newEngineer = new(engineerDetails.Item1, engineerDetails.Item2, engineerDetails.Item3, engineerDetails.Item4, engineerDetails.Item5);
            try
            {
                s_dal!.Engineer.Create(newEngineer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }

    // Create 20 tasks
    private static void createTasks() 
    {
        (string, string, string)[] detailsTask =
        {
            ("תכנון הבנייה על פי ייעוד ודרישות הבנין" ,"ארגון ותכנון הבניה", "תכנון הבניה הושלם"), //task 0
            ("הבטחת מימון פרויקט הבניה","מימון","מימון או מימון התחלתי לפרויקט הבניה"), //task 1
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
                TimeSpan.FromDays(1),
                false, // Milestone 
                null, // Start Date 
                null, // ScheduledDate date
                null, // DeadLine date
                null, // Complete date
                detailsTask[i].Item3,
                "Remarks for " + detailsTask[i].Item2,
                allEngineer[i]!.Id,
                allEngineer[i]!.Level
            );
            try
            {
                s_dal!.Task.Create(newTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    // Create 40 dependencies
    private static void createDependencies()
    {
        (int, int)[] dependencies = {
            (1 ,0), (2 ,0), (3 ,0), (4 ,3), (4 ,1), (4 ,2), (5 ,4), (6 ,5),
            (7 ,6), (8 ,7), (9 ,8), (10 ,9), (11 ,9), (12 ,9), (13 ,11), (14 ,11),
            (15 ,10), (15 ,11), (19 ,18), (20 ,19), (21 ,20), (21 ,17), (21 ,16), (21 ,15),
            (21 ,14), (21 ,12), (22 ,21)
        };

        foreach (var dependencyTuple in dependencies)
        {
            try
            {
                s_dal!.Dependency.Create(new Dependency(0, dependencyTuple.Item1, dependencyTuple.Item2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    public static void Do()
    {
        Factory.Get.Reset();
        s_dal = DalApi.Factory.Get; //stage 4
        createEngineers();
        createTasks();
        createDependencies();
    }
}