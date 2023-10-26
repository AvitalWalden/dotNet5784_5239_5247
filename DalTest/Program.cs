using Dal;
using DO;
using DalApi;
using System.Security.Cryptography;
using System.Reflection.Emit;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        // The function create a new task.
        public static void createTask()
        {
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine()!;
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine()!;
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine()!); //צריך להוריד את את !
            Console.WriteLine("Enter task schedule date");
            DateTime? scheduleDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine()!;
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine();
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine()!); //צריך להוריד את את !
            DateTime CreatedAt = DateTime.Now;  //????????????????????? איפה מהתחלים
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            /////////////////// מה זה האם הזימון טוב??
            DO.Task newTask = new DO.Task(0, description, alias, false, CreatedAt, startDate, scheduleDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            Console.WriteLine(s_dalTask!.Create(newTask)); // Input the new id of the task.
        }
        
        // The function update a task.
        public static void updateTask()
        {
            Console.WriteLine("Enter a ID of the task");
            Console.WriteLine("Enter a task ID");
            int idTask = int.Parse(Console.ReadLine()!);
            if (s_dalTask!.Read(idTask) != null)
            {
                Console.WriteLine(idTask);
            }
            else 
            {
                Console.WriteLine("Task with ID={idTask} not exists");/////////////////////////שחוזר לי ערך ריק, לזרוק שגיאה או להדפיס הודעה
            }
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine()!;
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine()!;
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine()!); //צריך להוריד את את !
            Console.WriteLine("Enter task schedule date");
            DateTime? scheduleDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);  //צריך להוריד את את !
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine()!;
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine();
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine()!); //צריך להוריד את את !
            DateTime CreatedAt = DateTime.Now;  //????????????????????? איפה מהתחלים
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            /////////////////// מה זה האם הזימון טוב??
            DO.Task newTask = new DO.Task(0, description, alias, false, CreatedAt, startDate, scheduleDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            try
            {
                s_dalTask!.Update(newTask); // Input the new id of the task.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all tasks
        public static void readAllTask()
        {
            List<DO.Task> tasks = s_dalTask!.ReadAll(); // ????/ DO.Task ????
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        // The function read a Task by ID
        public static void readTask(int idTask)
        {
            if (s_dalTask!.Read(idTask) == null) ////////////////////////////////////מה זה הסימן קריאה כאן חוזר לי מהפונקציה הזו ערך ריק האם הסימן קריאה משפיעה
            {
                Console.WriteLine("Task with ID={idTask} not exists");/////////////////////////שחוזר לי ערך ריק, לזרוק שגיאה או להדפיס הודעה
            }
            else
            {
                Console.WriteLine(s_dalTask!.Read(idTask));
            }
        }

        // The function delete a task.
        public static void deleteTask(int idTaskDelete)  /// ?? האם מותר למחוק משימה
        {
            try
            {
                s_dalTask!.Delete(idTaskDelete); // Input the new id of the task.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        // The function of the tasks
        public static void task()
        {
            Console.WriteLine("To add a task press a");
            Console.WriteLine("To read a task press b");
            Console.WriteLine("To read all tasks press c");
            Console.WriteLine("To update a task press d");
            Console.WriteLine("To delete a task press e"); //??????????????? האם אפשר למחוק
            char ch = char.Parse(Console.ReadLine()!);
            switch (ch)
            {
                case 'a': // Create a new task
                    createTask();
                    break;
                case 'b': // Read a Task by ID
                    Console.WriteLine("Enter a task ID");
                    int idTask = int.Parse(Console.ReadLine()!);
                    readTask(idTask);
                    break;
                case 'c': // Read all tasks
                    readAllTask();
                    break;
                case 'd': // Update a task.
                    updateTask();
                    break;
                case 'e': // Delete a task. //???????????????????// האם מותר למחוק משימה
                    Console.WriteLine("Enter a task ID");
                    int idTaskDelete = int.Parse(Console.ReadLine()!);
                    deleteTask(idTaskDelete);
                    break;
                default:
                    break;
            }
        }

        // The function create a new dependency.
        public static void CreatDependency()
        {
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine()!);
            /////////////////// מה זה האם הזימון טוב??
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            Console.WriteLine(s_dalDependency!.Create(newDependency)); // Input the new id of the new dependency.
        }

        // The function update a dependency.
        public static void updateDependency()
        {
            Console.WriteLine("Enter a dependency's ID");
            int idDependency = int.Parse(Console.ReadLine()!);
            if (s_dalTask!.Read(idDependency) != null)
            {
                Console.WriteLine(idDependency);
            }
            else
            {
                Console.WriteLine("Dependency with ID={idDependency} not exists");/////////////////////////שחוזר לי ערך ריק, לזרוק שגיאה או להדפיס הודעה
            }
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine()!);
            /////////////////// מה זה האם הזימון טוב??
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            try
            {
                s_dalDependency!.Update(newDependency); // Input the new id of the dependency.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all Dependencies
        public static void readAllDependencies()
        {
            List<DO.Dependency> dependencies = s_dalDependency!.ReadAll(); // ????/ DO.Task ????
            foreach (var dependency in dependencies)
            {
                Console.WriteLine(dependency);
            }
        }

        // The function read a dependency by ID
        public static void readDependency(int idDependency)
        {
            if (s_dalTask!.Read(idDependency) == null) ////////////////////////////////////מה זה הסימן קריאה כאן חוזר לי מהפונקציה הזו ערך ריק האם הסימן קריאה משפיעה
            {
                Console.WriteLine("Dependency with ID={idDependency} not exists");/////////////////////////שחוזר לי ערך ריק, לזרוק שגיאה או להדפיס הודעה
            }
            else
            {
                Console.WriteLine(s_dalTask!.Read(idDependency));
            }
        }

        // The function delete a dependency.
        public static void deleteDependency(int idDependency)
        {
            try
            {
                s_dalDependency!.Delete(idDependency); 
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        // The function of the Dependencies
        public static void Dependencies()
        {
            Console.WriteLine("To add depency between tasks press a");
            Console.WriteLine("To read depency between tasks press b");
            Console.WriteLine("To read all depency between tasks press c");
            Console.WriteLine("To update depency between tasks press d");
            Console.WriteLine("To delete depency between tasks press e"); //??????????????? האם אפשר למחוק
                                                                          // ch = char.Parse(Console.ReadLine()!);
                                                                          //  InfoOfOrderItem(x);//doing this function 
            char ch = char.Parse(Console.ReadLine()!);
            switch (ch)
            {
                case 'a': // Create a new Dependency
                    CreatDependency();
                    break;
                case 'b': // Read a dependency by ID
                    Console.WriteLine("Enter a dependency ID");
                    int idDependency = int.Parse(Console.ReadLine()!);
                    readDependency(idDependency);
                    break;
                case 'c': // Read all Dependencies
                    readAllDependencies();
                    break;
                case 'd': // Update a dependency.
                    updateDependency();
                    break;
                case 'e': // Delete a dependency. //???????????????????// האם מותר למחוק משימה
                    Console.WriteLine("Enter a Dependency ID");
                    int idDependencyDelete = int.Parse(Console.ReadLine()!);
                    deleteDependency(idDependencyDelete);
                    break;
                default:
                    break;
            }
        }


        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                Console.WriteLine("For a task press 1");
                Console.WriteLine("For an engineer press 2");
                Console.WriteLine("For depency between tasks press 3");
                Console.WriteLine("To exit press 0");
                int choose = int.Parse(Console.ReadLine()!);
                while (choose != 0)
                {
                    switch (choose)
                    {
                        case 1:
                            task();//doing this function
                            break;
                        case 2:
                            Console.WriteLine("To add an engineer press a");
                            Console.WriteLine("To read an engineer press b");
                            Console.WriteLine("To read all engineers press c");
                            Console.WriteLine("To update an engineer press d");
                            Console.WriteLine("To delete an engineer press e"); //??????????????? האם אפשר למחוק
                            //ch = char.Parse(Console.ReadLine()!);
                            //  InfoOfOrder(x); //doing this function 
                            break;
                        case 3:
                            Dependencies();
                            break;
                        default:
                            Console.WriteLine("The number entered is invalid");
                            break;
                    }
                    Console.WriteLine("enter a number");
                    choose = int.Parse(Console.ReadLine()!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}