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
        static readonly IDal s_dal = new DalList(); //stage 2

        // The function create a new task.
        public static void CreateTask()
        {
            Console.WriteLine("Enter a description of the task");
            string description = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter an alias of the task");
            string alias = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter task start date");
            DateTime? startDate = DateTime.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter task forecast date");
            DateTime? forecastDate = DateTime.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter task deadline date");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter task complete date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter the id of the engineer");
            int? engineerId = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            DO.Task newTask = new DO.Task(0, description, alias, false, startDate, forecastDate, deadlineDate, completeDate, productDescription, remarks, engineerId, (EngineerExperience)level);
            Console.WriteLine(s_dal!.Task.Create(newTask)); // Input the new id of the task.
        }

        // The function update a task.
        public static void UpdateTask()
        {
            Console.WriteLine("Enter a task's ID");
            int idTask = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            if (s_dal?.Task.Read(idTask) != null)
            {
                Console.WriteLine(s_dal?.Task.Read(idTask));
            }
            else
            {
                throw new DalDoesNotExistException($"Task with ID={idTask} not exists");
            }
            DO.Task updateTask = s_dal?.Task.Read(idTask)!;
            Console.WriteLine("Enter a description of the task");
            string? description = Console.ReadLine();
            if (description == "" || description == null)
            {
                description = updateTask.Description;
            }
            Console.WriteLine("Enter an alias of the task");
            string? alias = Console.ReadLine();
            if (alias == "" || alias == null)
            {
                alias = updateTask.Alias;
            }
            Console.WriteLine("Enter task start date");
            string? startDate1 = Console.ReadLine();
            DateTime? startDate;
            if (startDate1 == "" || startDate1 == null) //If not update the start date.
            {
                startDate = updateTask.Start;
            }
            else
            { 
                startDate = DateTime.Parse(startDate1);
            }
            Console.WriteLine("Enter task forecast date");
            string? forecastDate1 = Console.ReadLine();
            DateTime? forecastDate;
            if (forecastDate1 == "" || forecastDate1 == null) //If not update the forecast date.
            {
                forecastDate = updateTask.ForecastDate;
            }
            else
            {
                forecastDate = DateTime.Parse(forecastDate1);
            }
            Console.WriteLine("Enter task deadline date");
            string? deadlineDate1 = Console.ReadLine();
            DateTime? deadlineDate;
            if (deadlineDate1 == "" || deadlineDate1 == null) //If not update the forecast date.
            {
                deadlineDate = updateTask.Deadline;
            }
            else
            {
                deadlineDate = DateTime.Parse(deadlineDate1);
            }
            Console.WriteLine("Enter task complete date");
            string? completeDate1 = Console.ReadLine();
            DateTime? completeDate;
            if (completeDate1 == "" || completeDate1 == null) //If not update the complete date.
            {
                completeDate = updateTask.Complete;
            }
            else
            {
                completeDate = DateTime.Parse(completeDate1);
            }
            Console.WriteLine("Enter product description of the task");
            string? productDescription = Console.ReadLine();
            if (productDescription == "" || productDescription == null) //If not update the product description.
            {
                productDescription = updateTask.Deliverables;
            }
            Console.WriteLine("Enter remarks of the task");
            string? remarks = Console.ReadLine();
            if (remarks == "" || remarks == null) //If not update the remarks.
            {
                remarks = updateTask.Remarks;
            }
            Console.WriteLine("Enter the id of the engineer");
            string? id = Console.ReadLine();
            int? engineerId;
            if (id == null || id == "") //If not update the enginner id.
            {  
                engineerId = updateTask.EngineerId;
            }
            else
            {
                engineerId = int.Parse (id);
            }
            DateTime CreatedAt = DateTime.Now;
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            string? level1 = Console.ReadLine();
            int level;
            if (level1 == null || level1 == "") //If not update the level.
            {
                level = (int)updateTask.CopmlexityLevel!;
            }
            else
            {
                level = int.Parse(level1);
            }
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
            IEnumerable<DO.Task?> tasks = s_dal?.Task.ReadAll() ?? throw new DalDataListIsEmpty("There are no tasks.");
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
                throw new DalDoesNotExistException($"Task with ID={idTask} not exists");
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
            char ch = char.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            try
            {
                switch (ch)
                {
                    case 'a': // Create a new task.
                        CreateTask();
                        break;
                    case 'b': // Read a Task by ID.
                        Console.WriteLine("Enter a task ID");
                        int idTask = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
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
                        int idTaskDelete = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
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
            int id = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's name");
            string name = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter the engineer's email");
            string email = Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            int level = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter the engineer's cost");
            double cost = double.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            DO.Engineer newEngineer = new DO.Engineer(id, name, email, (EngineerExperience)level, cost);
            Console.WriteLine(s_dal!.Engineer.Create(newEngineer));
        }

        // The function update a engineer.
        public static void UpdateEngineer()
        {
            Console.WriteLine("Enter a engineer's ID");
            int id = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            if (s_dal?.Engineer.Read(id) != null)
            {
                Console.WriteLine(s_dal?.Engineer.Read(id));
            }
            else
            {
                throw new DalDoesNotExistException($"Engineer with ID={id} not exists");
            }
            Engineer updateEngineer = s_dal?.Engineer.Read(id)!;
            Console.WriteLine("Enter the engineer's name");
            string? name = Console.ReadLine(); //If not update the name.
            if (name == "" || name == null)
            {
                name = updateEngineer.Name;
            }
            Console.WriteLine("Enter the engineer's email");
            string? email = Console.ReadLine();
            if (email == "" || email == null)
            {
                email = updateEngineer.Email;
            }
            Console.WriteLine("Enter the level of the task:");
            Console.WriteLine("For expert press 0");
            Console.WriteLine("For junior press 1");
            Console.WriteLine("For rookie press 2");
            string? level1 = Console.ReadLine();
            int level;
            if (level1 == null || level1 == "") //If not update the level.
            {
                level = (int)updateEngineer.Level;
            }
            else
            {
                level = int.Parse(level1);
            }
            Console.WriteLine("Enter the engineer's cost");
            string? cost1 = Console.ReadLine();
            double cost;
            if (cost1 == null || cost1 == "") //If not update the cost.
            {
                cost = (double)updateEngineer.Cost;
            }
            else
            {
                cost = double.Parse(cost1);
            }
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
                throw new DalDoesNotExistException($"Engineer with ID={idEngineer} not exists");
            }
            else
            {
                Console.WriteLine(s_dal?.Engineer.Read(idEngineer));
            }
        }

        // The function read all the engineers.
        public static void ReadAllEngineers()
        {
            IEnumerable<DO.Engineer?> engineers = s_dal?.Engineer.ReadAll() ?? throw new DalDataListIsEmpty("There are no engineers.");
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
            char ch = char.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            try
            {
                switch (ch)
                {
                    case 'a': // Create a new engineer.
                        CreateEngineer();
                        break;
                    case 'b': // Read a engineer by ID
                        Console.WriteLine("Enter a engineer's id");
                        int idEngineer = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
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
                        int idEngineerDelete = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
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
            int? dependentTask = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            Console.WriteLine("Enter ID number of a previous assignment");
            int? dependsOnTask = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            DO.Dependency newDependency = new DO.Dependency(0, dependentTask, dependsOnTask);
            Console.WriteLine(s_dal?.Dependency.Create(newDependency)); // Input the new id of the new dependency.
        }

        // The function update a dependency.
        public static void UpdateDependency()
        {
            Console.WriteLine("Enter a dependency's ID");
            int idDependency = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            if (s_dal!.Dependency.Read(idDependency) != null)
            {
                Console.WriteLine(s_dal!.Dependency.Read(idDependency));
            }
            else
            {
                throw new DalDoesNotExistException($"Dependency with ID={idDependency} not exists");
            }
            Dependency updateDependency = s_dal!.Dependency.Read(idDependency)!;
            Console.WriteLine("Enter ID number of pending task");
            string? dependentTask1 = Console.ReadLine();
            int? dependentTask;
            if (dependentTask1 == null || dependentTask1 == "") //If not update the dependent task.
            {
                dependentTask = updateDependency.DependentTask;
            }
            else
            {
                dependentTask = int.Parse(dependentTask1);
            }
            Console.WriteLine("Enter ID number of a previous assignment");
            string? dependsOnTask1 = Console.ReadLine();
            int? dependsOnTask;
            if (dependsOnTask1 == null || dependsOnTask1 == "") //If not update the dependent on task.
            {
                dependsOnTask = updateDependency.DependsOnTask;
            }
            else
            {
                dependsOnTask = int.Parse(dependsOnTask1);
            }
            DO.Dependency newDependency = new DO.Dependency(idDependency, dependentTask, dependsOnTask);
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
        public static void ReadAllDependencies()
        {
            IEnumerable<DO.Dependency?> dependencies = s_dal?.Dependency.ReadAll() ?? throw new DalDataListIsEmpty("There are no dependencies.");
            foreach (var dependency in dependencies)
            {
                Console.WriteLine(dependency);
            }
        }

        // The function read a dependency by ID.
        public static void ReadDependency(int idDependency)
        {
            if (s_dal?.Dependency.Read(idDependency) == null)
            {
                throw new DalInvalidEnteredValue($"Dependency with ID={idDependency} not exists");
            }
            else
            {
                Console.WriteLine(s_dal?.Dependency.Read(idDependency));
            }
        }

        // The function delete a dependency.
        public static void DeleteDependency(int idDependency)
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
            char ch = char.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
            try
            {
                switch (ch)
                {
                    case 'a': // Create a new Dependency
                        CreatDependency();
                        break;
                    case 'b': // Read a dependency by ID
                        Console.WriteLine("Enter a dependency ID");
                        int idDependency = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
                        ReadDependency(idDependency);
                        break;
                    case 'c': // Read all Dependencies
                        ReadAllDependencies();
                        break;
                    case 'd': // Update a dependency.
                        UpdateDependency();
                        break;
                    case 'e': // Delete a dependency.
                        Console.WriteLine("Enter a Dependency ID");
                        int idDependencyDelete = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
                        DeleteDependency(idDependencyDelete);
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


        static void Main(string[] args)
        {
           
                //Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                Initialization.Do(s_dal); //stage 2
                Console.WriteLine("For a task press 1");
                Console.WriteLine("For an engineer press 2");
                Console.WriteLine("For depency between tasks press 3");
                Console.WriteLine("To exit press 0");
            int choose = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
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
                        Console.WriteLine();
                        Console.WriteLine("enter a number:");
                        Console.WriteLine("For a task press 1");
                        Console.WriteLine("For an engineer press 2");
                        Console.WriteLine("For depency between tasks press 3");
                        Console.WriteLine("To exit press 0");
                        choose = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
        }
    }
}
