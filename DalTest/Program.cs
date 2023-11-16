using Dal;
using DO;
using DalApi;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace DalTest
{
    internal class Program
    {
        //private static IEngineer? s_dalEngineer = new EngineerImplementation();
        //private static ITask? s_dalTask = new TaskImplementation();
        //private static IDependency? s_dalDependency = new DependencyImplementation();
        static readonly IDal s_dal = new DalList(); //stage 2

        // The function create a new task.
        public static void CreateTask()
        {
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine()!;
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine()!;
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine()!;
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine()!;
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine()!);
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            DO.Task newTask = new DO.Task(0, description, alias, false, startDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            Console.WriteLine(s_dal!.Task.Create(newTask)); // Input the new id of the task.
        }
        
        // The function update a task.
        public static void UpdateTask()
        {
            Console.WriteLine("Enter a task's ID");
            int idTask = int.Parse(Console.ReadLine()!);
            if (s_dal?.Task.Read(idTask) != null)
            {
                Console.WriteLine(s_dal?.Task.Read(idTask));
            }
            else 
            {
                throw new Exception($"Task with ID={idTask} not exists");
            }
            DO.Task updateTask = s_dal?.Task.Read(idTask)!;
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine()!;
            if (description == "" || description == null)
            {
                description = updateTask.Description;
            }
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine()!;
            if (alias == "" || alias == null)
            {
                alias = updateTask.Alias;
            }
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine()!);
            if (!startDate.HasValue) // If the input was incorrect or not entered
            {  startDate = updateTask.Start; }
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine()!);
            if (!forecastDate.HasValue) // If the input was incorrect or not entered
            { forecastDate = updateTask.ForecastDate; }
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);
            if (!deadlineDate.HasValue) // If the input was incorrect or not entered
            { deadlineDate = updateTask.Deadline; }
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);
            if (!deadlineDate.HasValue) // If the input was incorrect or not entered
            { deadlineDate = updateTask.Complete; }
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine()!;
            if (productDescription == "" || productDescription == null)
            {
                productDescription = updateTask.Deliverables;
            }
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine()!;
            if (remarks == "" || remarks == null)
            {
                remarks = updateTask.Remarks;
            }
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine()!);
            if (engineerId == null)
            {  engineerId = updateTask.EngineerId;}
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            if (level !=  1 || level != 2 || level != 3 )
            { level = (int)updateTask.CopmlexityLevel!; }
            DO.Task newTask = new DO.Task(idTask, description, alias, false, startDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            try
            {
                s_dal?.Task.Update(newTask); // Input the new id of the task.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all tasks.
        public static void ReadAllTasks()
        {
            List<DO.Task> tasks = s_dal?.Task.ReadAll() ?? throw new Exception("There are no tasks.");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        // The function read a Task by ID.
        public static void ReadTask(int idTask)
        {
            if (s_dal?.Task.Read(idTask) == null)
            {
                throw new Exception($"Task with ID={idTask} not exists");
            }
            else
            {
                Console.WriteLine(s_dal?.Task.Read(idTask));
            }
        }

        // The function delete a task.
        public static void DeleteTask(int idTaskDelete)
        {
            try
            {
                s_dal?.Task.Delete(idTaskDelete); // Input the new id of the task.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        // The function of the tasks.
        public static void Tasks()
        {
            Console.WriteLine("To add a task press a");
            Console.WriteLine("To read a task press b");
            Console.WriteLine("To read all tasks press c");
            Console.WriteLine("To update a task press d");
            Console.WriteLine("To delete a task press e");
            Console.WriteLine("To exit press f");
            char ch = char.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            try
            {
                switch (ch)
                {
                    case 'a': // Create a new task.
                        CreateTask();
                        break;
                    case 'b': // Read a Task by ID.
                        Console.WriteLine("Enter a task ID");
                        int idTask = int.Parse(Console.ReadLine()!);
                        ReadTask(idTask);
                        break;
                    case 'c': // Read all tasks.
                        ReadAllTasks();
                        break;
                    case 'd': // Update a task.
                        UpdateTask();
                        break;
                    case 'e': // Delete a task. 
                        Console.WriteLine("Enter a task ID");
                        int idTaskDelete = int.Parse(Console.ReadLine()!);
                        DeleteTask(idTaskDelete);
                        break;
                    case 'f':
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex); ;
            }
        }

        // The function create a new engineer.
        public static void CreateEngineer()
        {
            Console.WriteLine("Enter the engineer's id");
            int id = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter the engineer's name");
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter the engineer's email");
            string email = Console.ReadLine()!;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter the engineer's cost");
            double cost = double.Parse(Console.ReadLine()!);
            DO.Engineer newEngineer = new DO.Engineer(id, name, email, (EngineerExperience)level, cost);
            Console.WriteLine(s_dal!.Engineer.Create(newEngineer));
        }

        // The function update a engineer.
        public static void UpdateEngineer()
        {
            Console.WriteLine("Enter a engineer's ID");
                int id = int.Parse(Console.ReadLine()!);
                if (s_dal?.Engineer.Read(id) != null)
            {
                Console.WriteLine(s_dal?.Engineer.Read(id));
            }
            else
            {
                throw new Exception($"Engineer with ID={id} not exists");
            }
            Engineer updateEngineer = s_dal?.Engineer.Read(id)!;
            Console.WriteLine("Enter the engineer's name");
            string name = Console.ReadLine()!;
            if (name == "" || name == null)
            {
                name = updateEngineer.Name;
            }
            Console.WriteLine("Enter the engineer's email");
            string email = Console.ReadLine()!;
            if (email == "" || email == null)
            {
                email = updateEngineer.Email;
            }
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter the engineer's cost");
            if (level != 0 || level != 1 || level != 2)
            { level = (int)updateEngineer.level!; }
            double cost = double.Parse(Console.ReadLine()!);
            DO.Engineer newEngineer = new DO.Engineer(id, name, email, (EngineerExperience)level, cost);
            try
            {
                s_dal?.Engineer.Update(newEngineer);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read a engineer by ID.
        public static void ReadEngineer(int idEngineer)
        {
            if (s_dal?.Engineer.Read(idEngineer) == null)
            {
                throw new Exception($"Engineer with ID={idEngineer} not exists");
            }
            else
            {
                Console.WriteLine(s_dal?.Engineer.Read(idEngineer));
            }
        }

        // The function read all the engineers.
        public static void ReadAllEngineers()
        {
            List<DO.Engineer> engineers = s_dal?.Engineer.ReadAll() ?? throw new Exception("There are no engineers.");
            foreach (var engineer in engineers)
            {
                Console.WriteLine(engineer);
            }
        }

        // The function delete a engineer.
        public static void DeleteEngineer(int idEngineerDelete)
        {
            try
            {
                s_dal?.Engineer.Delete(idEngineerDelete); // Input the new id of the task.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // The function of the engineer.
        public static void Engineers()
        {
            Console.WriteLine("To add an engineer press a");
            Console.WriteLine("To read an engineer press b");
            Console.WriteLine("To read all engineers press c");
            Console.WriteLine("To update an engineer press d");
            Console.WriteLine("To delete an engineer press e");
            Console.WriteLine("To exit press f");
            char ch = char.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            try
            {
                switch (ch)
                {
                    case 'a': // Create a new engineer.
                        CreateEngineer();
                        break;
                    case 'b': // Read a engineer by ID
                        Console.WriteLine("Enter a engineer's id");
                        int idEngineer = int.Parse(Console.ReadLine()!);
                        ReadEngineer(idEngineer);
                        break;
                    case 'c': // Read all engineers.
                        ReadAllEngineers();
                        break;
                    case 'd': // Update a engineer.
                        UpdateEngineer();
                        break;
                    case 'e': // Delete a engineer.
                        Console.WriteLine("Enter a engineer's ID");
                        int idEngineerDelete = int.Parse(Console.ReadLine()!);
                        DeleteEngineer(idEngineerDelete);
                        break;
                    case 'f':
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex); ;
            }
        }

        // The function create a new dependency.
        public static void CreatDependency()
        {
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine()!);
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            Console.WriteLine(s_dal?.Dependency.Create(newDependency)); // Input the new id of the new dependency.
        }

        // The function update a dependency.
        public static void updateDependency()
        {
            Console.WriteLine("Enter a dependency's ID");
            int idDependency = int.Parse(Console.ReadLine()!);
            if (s_dal!.Dependency.Read(idDependency) != null)
            {
                Console.WriteLine(s_dal!.Dependency.Read(idDependency));
            }
            else
            {
                throw new Exception($"Dependency with ID={idDependency} not exists");
            }
            Dependency updateDependency = s_dal!.Dependency.Read(idDependency)!;
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine()!);
            if(dependentTask == null) {
                dependentTask = updateDependency.DependentTask;
            }
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine()!);
            if (dependsOnTask == null)
            {
                dependsOnTask = updateDependency.DependsOnTask;
            }
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            try
            {
                s_dal?.Dependency.Update(newDependency); // Input the new id of the dependency.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all Dependencies.
        public static void readAllDependencies()
        {
            List<DO.Dependency> dependencies = s_dal?.Dependency.ReadAll() ?? throw new Exception("There are no dependencies.");
            foreach (var dependency in dependencies)
            {
                Console.WriteLine(dependency);
            }
        }

        // The function read a dependency by ID.
        public static void readDependency(int idDependency)
        {
            if (s_dal?.Dependency.Read(idDependency) == null)
            {
                throw new Exception($"Dependency with ID={idDependency} not exists");
            }
            else
            {
                Console.WriteLine(s_dal?.Dependency.Read(idDependency));
            }
        }

        // The function delete a dependency.
        public static void deleteDependency(int idDependency)
        {
            try
            {
                s_dal?.Dependency.Delete(idDependency); 
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
            Console.WriteLine("To delete depency between tasks press e");
            Console.WriteLine("To exit press f");
            char ch = char.Parse(Console.ReadLine()!);
            try
            {
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
                    case 'e': // Delete a dependency.
                        Console.WriteLine("Enter a Dependency ID");
                        int idDependencyDelete = int.Parse(Console.ReadLine()!);
                        deleteDependency(idDependencyDelete);
                        break;
                    case 'f':
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex); ;
            }
        }


        static void Main(string[] args)
        {
           
                //Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                Initialization.Do(s_dal); //stage 2
                Console.WriteLine("For a task press 1");
                Console.WriteLine("For an engineer press 2");
                Console.WriteLine("For depency between tasks press 3");
                Console.WriteLine("To exit press 0");
                int choose = int.Parse(Console.ReadLine()!);
                try
                {
                    while (choose != 0)
                    {
                        switch (choose)
                        {
                            case 1:
                                Tasks();
                                break;
                            case 2:
                                Engineers();
                                break;
                            case 3:
                                Dependencies();
                                break;
                            default:
                                Console.WriteLine("The number entered is invalid");
                                break;
                        }
                        Console.WriteLine("enter a number:");
                        Console.WriteLine("For a task press 1");
                        Console.WriteLine("For an engineer press 2");
                        Console.WriteLine("For depency between tasks press 3");
                        Console.WriteLine("To exit press 0");
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