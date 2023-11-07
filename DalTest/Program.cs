﻿using Dal;
using DO;
using DalApi;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        // The function create a new task.
        public static void CreateTask()
        {
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine()!;
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine()!;
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task schedule date");
            DateTime? scheduleDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine()!);
            DO.Task newTask = new DO.Task(0, description, alias, false, startDate, scheduleDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            Console.WriteLine(s_dalTask!.Create(newTask)); // Input the new id of the task.
        }
        
        // The function update a task.
        public static void UpdateTask()
        {
            Console.WriteLine("Enter a task's ID");
            int idTask = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            if (s_dalTask?.Read(idTask) != null)
            {
                Console.WriteLine(s_dalTask?.Read(idTask));
            }
            else 
            {
                throw new Exception("Task with ID={idTask} not exists");
            }
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task schedule date");
            DateTime? scheduleDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine();
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DO.Task newTask = new DO.Task(idTask, description, alias, false, startDate, scheduleDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            try
            {
                s_dalTask!.Update(newTask); // Input the new id of the task.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all tasks.
        public static void ReadAllTasks()
        {
            List<DO.Task> tasks = s_dalTask?.ReadAll() ?? throw new Exception("There are no tasks.");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        // The function read a Task by ID.
        public static void ReadTask(int idTask)
        {
            if (s_dalTask?.Read(idTask) == null)
            {
                throw new Exception("Task with ID={idTask} not exists");
            }
            else
            {
                Console.WriteLine(s_dalTask?.Read(idTask));
            }
        }

        // The function delete a task.
        public static void DeleteTask(int idTaskDelete)
        {
            try
            {
                s_dalTask?.Delete(idTaskDelete); // Input the new id of the task.
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

        // The function create a new engineer.
        public static void CreateEngineer()
        {
            Console.WriteLine("Enter the engineer's id");
            int id = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's name");
            string name = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter the engineer's email");
            string email = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's cost");
            double cost = double.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DO.Engineer newEngineer = new DO.Engineer(id, name, email, (EngineerExperience)level, cost);
            Console.WriteLine(s_dalEngineer!.Create(newEngineer));
        }

        // The function update a engineer.
        public static void UpdateEngineer()
        {
            Console.WriteLine("Enter a engineer's ID");
            int idEngineer = int.Parse(Console.ReadLine()!);
            if (s_dalEngineer?.Read(idEngineer) != null)
            {
                Console.WriteLine(idEngineer);
            }
            else
            {
                throw new Exception("Engineer with ID={idEngineer} not exists");
            }
            Console.WriteLine("Enter the engineer's id");
            int id = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's name");
            string name = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter the engineer's email");
            string email = Console.ReadLine() ?? throw new Exception("The entered value is incorrect");
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's cost");
            double cost = double.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DO.Engineer newEngineer = new DO.Engineer(id, name, email, (EngineerExperience)level, cost);
            try
            {
                s_dalEngineer?.Update(newEngineer);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read a engineer by ID.
        public static void ReadEngineer(int idEngineer)
        {
            if (s_dalEngineer?.Read(idEngineer) == null)
            {
                throw new Exception("Engineer with ID={idTask} not exists");
            }
            else
            {
                Console.WriteLine(s_dalEngineer?.Read(idEngineer));
            }
        }

        // The function read all the engineers.
        public static void ReadAllEngineers()
        {
            List<DO.Engineer> engineers = s_dalEngineer?.ReadAll() ?? throw new Exception("There are no engineers.");
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
                s_dalEngineer?.Delete(idEngineerDelete); // Input the new id of the task.
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

        // The function create a new dependency.
        public static void CreatDependency()
        {
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            Console.WriteLine(s_dalDependency?.Create(newDependency)); // Input the new id of the new dependency.
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
                throw new Exception("Dependency with ID={idDependency} not exists");
            }
            Console.WriteLine("Enter ID number of pending task");
            int? dependentTask = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine() ?? throw new Exception("The entered value is incorrect"));
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            try
            {
                s_dalDependency?.Update(newDependency); // Input the new id of the dependency.
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        // The function read all Dependencies.
        public static void readAllDependencies()
        {
            List<DO.Dependency> dependencies = s_dalDependency?.ReadAll() ?? throw new Exception("There are no dependencies.");
            foreach (var dependency in dependencies)
            {
                Console.WriteLine(dependency);
            }
        }

        // The function read a dependency by ID.
        public static void readDependency(int idDependency)
        {
            if (s_dalTask?.Read(idDependency) == null)
            {
                throw new Exception("Dependency with ID={idDependency} not exists");
            }
            else
            {
                Console.WriteLine(s_dalTask?.Read(idDependency));
            }
        }

        // The function delete a dependency.
        public static void deleteDependency(int idDependency)
        {
            try
            {
                s_dalDependency?.Delete(idDependency); 
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


        static void Main(string[] args)
        {
           
                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                Console.WriteLine("For a task press 1");
                Console.WriteLine("For an engineer press 2");
                Console.WriteLine("For depency between tasks press 3");
                Console.WriteLine("To exit press 0");
                int choose = int.Parse(Console.ReadLine()!);
                while (choose != 0)
                {
                      try
                      {
                            switch (choose)
                            {
                                case 1:
                                    Tasks();//doing this function
                                    break;
                                case 2:
                                    Engineers(); //doing this function 
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
                      catch (Exception ex)
                      {
                           Console.WriteLine(ex);
                      }   
                }
        }
    }
}