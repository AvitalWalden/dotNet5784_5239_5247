using BO;
using DalApi;



internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // The function create a new task.
    public static void CreateTask()
    {
        Console.WriteLine("Enter a description of the task");
        string description = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter an alias of the task");
        string alias = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter required effort time to the tast");
        TimeSpan requiredEffort = TimeSpan.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter task start date");
        DateTime? startDate = DateTime.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter task Scheduled date");
        DateTime? ScheduledDate = DateTime.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter task deadline date");
        DateTime? deadlineDate = DateTime.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter task complete date");
        DateTime? completeDate = DateTime.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter product deliverables of the task");
        string? deliverables = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter remarks of the task");
        string? remarks = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter the id of the engineer");
        int? engineerId = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        DateTime createdAt = DateTime.Now;
        Console.WriteLine("Enter the level of the task:");
        Console.WriteLine("For Beginner press 0");
        Console.WriteLine("For AdvancedBeginner press 1");
        Console.WriteLine("For Competent press 2");
        Console.WriteLine("For Proficient press 3");
        Console.WriteLine("For Expert press 4");
        int level = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
      //  DO.Task newTask = new DO.Task(0, description, alias, createdAt, requiredEffort, false, startDate, ScheduledDate, deadlineDate, completeDate, deliverables, remarks, engineerId, (EngineerExperience)level);
       // Console.WriteLine(s_bl!.Task.Create(newTask)); // Input the new id of the task.
    }

    // The function update a task.
    public static void UpdateTask()
    {
        //Console.WriteLine("Enter a task's ID");
        //int idTask = int.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
        //if (s_bl?.Task.Read(idTask) != null)
        //{
        //    Console.WriteLine(s_bl?.Task.Read(idTask));
        //}
        //else
        //{
        //    throw new DalDoesNotExistException($"Task with ID={idTask} not exists");
        //}
        //DO.Task updateTask = s_bl?.Task.Read(idTask)!;
        //Console.WriteLine("Enter a description of the task");
        //string? description = Console.ReadLine();
        //if (description == "" || description == null)
        //{
        //    description = updateTask.Description;
        //}
        //Console.WriteLine("Enter an alias of the task");
        //string? alias = Console.ReadLine();
        //if (alias == "" || alias == null)
        //{
        //    alias = updateTask.Alias;
        //}
        //Console.WriteLine("Enter required effort time to the tast");
        //string requiredEffort1 = Console.ReadLine()!;
        //TimeSpan requiredEffort;
        //if (requiredEffort1 == "" || requiredEffort1 == null) //If not update the start date.
        //{
        //    requiredEffort = updateTask.RequiredEffort;
        //}
        //else
        //{
        //    requiredEffort = TimeSpan.Parse(requiredEffort1);
        //}
        //Console.WriteLine("Enter task start date");
        //string? startDate1 = Console.ReadLine();
        //DateTime? startDate;
        //if (startDate1 == "" || startDate1 == null) //If not update the start date.
        //{
        //    startDate = updateTask.StartDate;
        //}
        //else
        //{
        //    startDate = DateTime.Parse(startDate1);
        //}
        //Console.WriteLine("Enter task scheduled date");
        //string? scheduledDate1 = Console.ReadLine();
        //DateTime? scheduledDate;
        //if (scheduledDate1 == "" || scheduledDate1 == null) //If not update the forecast date.
        //{
        //    scheduledDate = updateTask.ScheduledDate;
        //}
        //else
        //{
        //    scheduledDate = DateTime.Parse(scheduledDate1);
        //}
        //Console.WriteLine("Enter task deadline date");
        //string? deadlineDate1 = Console.ReadLine();
        //DateTime? deadlineDate;
        //if (deadlineDate1 == "" || deadlineDate1 == null) //If not update the forecast date.
        //{
        //    deadlineDate = updateTask.DeadlineDate;
        //}
        //else
        //{
        //    deadlineDate = DateTime.Parse(deadlineDate1);
        //}
        //Console.WriteLine("Enter task complete date");
        //string? completeDate1 = Console.ReadLine();
        //DateTime? completeDate;
        //if (completeDate1 == "" || completeDate1 == null) //If not update the complete date.
        //{
        //    completeDate = updateTask.CompleteDate;
        //}
        //else
        //{
        //    completeDate = DateTime.Parse(completeDate1);
        //}
        //Console.WriteLine("Enter deliverables of the task");
        //string? deliverables = Console.ReadLine();
        //if (deliverables == "" || deliverables == null) //If not update the product description.
        //{
        //    deliverables = updateTask.Deliverables;
        //}
        //Console.WriteLine("Enter remarks of the task");
        //string? remarks = Console.ReadLine();
        //if (remarks == "" || remarks == null) //If not update the remarks.
        //{
        //    remarks = updateTask.Remarks;
        //}
        //Console.WriteLine("Enter the id of the engineer");
        //string? id = Console.ReadLine();
        //int? engineerId;
        //if (id == null || id == "") //If not update the enginner id.
        //{
        //    engineerId = updateTask.EngineerId;
        //}
        //else
        //{
        //    engineerId = int.Parse(id);
        //}
        //DateTime CreatedAt = DateTime.Now;
        //Console.WriteLine("Enter the level of the task:");
        //Console.WriteLine("For Beginner press 0");
        //Console.WriteLine("For AdvancedBeginner press 1");
        //Console.WriteLine("For Competent press 2");
        //Console.WriteLine("For Proficient press 3");
        //Console.WriteLine("For Expert press 4");
        //string? level1 = Console.ReadLine();
        //int level;
        //if (level1 == null || level1 == "") //If not update the level.
        //{
        //    level = (int)updateTask.ComplexityLevel!;
        //}
        //else
        //{
        //    level = int.Parse(level1);
        //}
        //Console.WriteLine("Enter if the task is active or not(Y/N)");
        //bool active;
        //string active1 = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //if (active1 == null || active1 == "") //If not update the cost.
        //{
        //    active = updateTask.Active;
        //}
        //else
        //{
        //    if (active1 == "Y")
        //        active = true;
        //    else active = false;
        //}
        //DO.Task newTask = new DO.Task(idTask, description, alias, updateTask.CreatedAtDate, requiredEffort, false, startDate, scheduledDate, deadlineDate, completeDate, deliverables, remarks, engineerId, (EngineerExperience)level, active);
        //try
        //{
        //   s_bl?.Task.Update(newTask); // Input the new id of the task.
        //}
        //catch (Exception ex)
        //{

        //    Console.WriteLine(ex);
        //}

    }

    // The function read all tasks.
    public static void ReadAllTasks()
    {
        //IEnumerable<DO.Task?> tasks = s_bl?.Task.ReadAll() ?? throw new DalDataListIsEmpty("There are no tasks.");
        //foreach (var task in tasks)
        //{
        //    Console.WriteLine(task);
        //}
    }

    // The function read a Task by ID.
    public static void ReadTask(int idTask)
    {
        //if (s_bl?.Task.Read(idTask) == null)
        //{
        //    throw new DalDoesNotExistException($"Task with ID={idTask} not exists");
        //}
        //else
        //{
        //    Console.WriteLine(s_bl?.Task.Read(idTask));
        //}
    }

    // The function delete a task.
    public static void DeleteTask(int idTaskDelete)
    {
        try
        {
            s_bl?.Task.Delete(idTaskDelete); // Input the new id of the task.
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }
    }

    // The function of the tasks.
    // The function of the tasks.
    public static void Tasks()
    {
        Console.WriteLine("To add a task press a");
        Console.WriteLine("To read a task press b");
        Console.WriteLine("To read all tasks press c");
        Console.WriteLine("To update a task press d");
        Console.WriteLine("To delete a task press e");
        Console.WriteLine("To exit press f");
        char ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        try
        {
            while (ch != 'f')
            {
                switch (ch)
                {
                    case 'a': // Create a new task.
                        CreateTask();
                        break;
                    case 'b': // Read a Task by ID.
                        Console.WriteLine("Enter a task ID");
                        int idTask = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
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
                        int idTaskDelete = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
                        DeleteTask(idTaskDelete);
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("To add a task press a");
                Console.WriteLine("To read a task press b");
                Console.WriteLine("To read all tasks press c");
                Console.WriteLine("To update a task press d");
                Console.WriteLine("To delete a task press e");
                Console.WriteLine("To exit press f");
                ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }

    }

    /// <summary>
    ///The function create a new task.
    /// </summary>
    /// <exception cref="BlInvalidEnteredValue">exception: The entered value is incorrect</exception>
    public static void CreateEngineer()
    {
        Console.WriteLine("Enter the engineer's id");
        int id = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter the engineer's name");
        string name = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter the engineer's email");
        string email = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter the level of the task:");
        Console.WriteLine("For Beginner press 0");
        Console.WriteLine("For AdvancedBeginner press 1");
        Console.WriteLine("For Competent press 2");
        Console.WriteLine("For Proficient press 3");
        Console.WriteLine("For Expert press 4");
        int level = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter the engineer's cost");
        double cost = double.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter the id's task of this engineer");
        string? chooseBeforeParse = Console.ReadLine();
        int.TryParse(chooseBeforeParse, out int idOfTask);
        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = (BO.EngineerExperience)level,
            Cost = cost,
            Task = new BO.TaskInEngineer() 
            {
               Id = idOfTask,
               Alias = s_bl.Task.Read(idOfTask)?.Alias!
            }
        };
        Console.WriteLine(s_bl!.Engineer.Create(newEngineer));
    }

    /// <summary>
    ///  The function update a engineer.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">exception: The entered value is incorrect</exception>
    /// <exception cref="BO.BlDoesNotExistException">exception: The engineer not exists</exception>
    public static void UpdateEngineer()
    {
        Console.WriteLine("Enter a engineer's ID");
        int id = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        if (s_bl?.Engineer.Read(id) != null)
        {
            Console.WriteLine(s_bl?.Engineer.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} not exists");
        }
        BO.Engineer updateEngineer = s_bl?.Engineer.Read(id)!;
        Console.WriteLine("Enter the engineer's name");
        string? name = Console.ReadLine();
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
        Console.WriteLine("For Beginner press 0");
        Console.WriteLine("For AdvancedBeginner press 1");
        Console.WriteLine("For Competent press 2");
        Console.WriteLine("For Proficient press 3");
        Console.WriteLine("For Expert press 4");
        string? levelBeforeParse = Console.ReadLine();
        int.TryParse(levelBeforeParse, out int level);
        if (levelBeforeParse == null || levelBeforeParse == "") //If not update the level.
        {
            level = (int)updateEngineer.Level;
        }
        Console.WriteLine("Enter the engineer's cost");
        string? costBeforeParse = Console.ReadLine();
        double.TryParse(levelBeforeParse, out double cost);
        if (costBeforeParse == null || costBeforeParse == "") //If not update the cost.
        {
            cost = (double)updateEngineer.Cost;
        }
        Console.WriteLine("Enter if the engineer is active or not(Y/N)");
        string? activeBeforeParse = Console.ReadLine();
        double.TryParse(levelBeforeParse, out double active);
        //if (activeBeforeParse == null || activeBeforeParse == "") //If not update the cost.
        //{
        //    active = updateEngineer.Active;
        //}
        //else
        //{
        //    if (active1 == "Y")
        //        active = true;
        //    else active = false;
        //}
        Console.WriteLine("Enter the id's task of this engineer");
        string? chooseBeforeParse = Console.ReadLine();
        int.TryParse(chooseBeforeParse, out int idOfTask);
        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = (BO.EngineerExperience)level,
            Cost = cost,
            Task = new BO.TaskInEngineer()
            {
                Id = idOfTask,
                Alias = s_bl?.Task.Read(idOfTask)?.Alias!
            }
        };
        try
        {
            s_bl?.Engineer.Update(newEngineer);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    /// <summary>
    ///  The function read a engineer by ID.
    /// </summary>
    /// <param name="idEngineer">The id engineer to read</param>
    /// <exception cref="BlDoesNotExistException">"exception: engineer with this id does not exists</exception>
    public static void ReadEngineer(int idEngineer)
    {
        if (s_bl?.Engineer.Read(idEngineer) == null)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={idEngineer} not exists");
        }
        else
        {
            Console.WriteLine(s_bl?.Engineer.Read(idEngineer));
        }
    }

    /// <summary>
    /// The function read all the engineers.
    /// </summary>
    /// <exception cref="BO.BlDataListIsEmpty">The list is empty. There is no data to read.</exception>
    public static void ReadAllEngineers()
    {
        IEnumerable<BO.Engineer?> engineers = s_bl?.Engineer.ReadAll() ?? throw new BO.BlDataListIsEmpty("There are no engineers.");
        foreach (var engineer in engineers)
        {
            Console.WriteLine(engineer);
        }
    }

    /// <summary>
    /// The function delete a engineer.
    /// </summary>
    /// <param name="idEngineerDelete">The id of engineer to delete</param>
    public static void DeleteEngineer(int idEngineerDelete)
    {
        try
        {
            s_bl?.Engineer.Delete(idEngineerDelete); // Input the new id of the task.
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
        char ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        try
        {
            while (ch != 'f')
            {
                switch (ch)
                {
                    case 'a': // Create a new engineer.
                        CreateEngineer();
                        break;
                    case 'b': // Read a engineer by ID
                        Console.WriteLine("Enter a engineer's id");
                        int idEngineer = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
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
                        int idEngineerDelete = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
                        DeleteEngineer(idEngineerDelete);
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("To add an engineer press a");
                Console.WriteLine("To read an engineer press b");
                Console.WriteLine("To read all engineers press c");
                Console.WriteLine("To update an engineer press d");
                Console.WriteLine("To delete an engineer press e");
                Console.WriteLine("To exit press f");
                ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex); ;
        }
    }

    public static void CreateMilestone()
    {

    }
    public static void ReadMilestone(int idMilestone)
    {
        if (s_bl?.Milestone.Read(idMilestone) == null)
        {
            throw new BO.BlDoesNotExistException($"Milestone with ID={idMilestone} not exists");
        }
        else
        {
            Console.WriteLine(s_bl?.Milestone.Read(idMilestone));
        }
    }
    public static void UpdateMilestone()
    {

    }
    public static void Milestones()
    {
        Console.WriteLine("To add a task press a");
        Console.WriteLine("To read a task press b");
        Console.WriteLine("To update a milestone press c");
        Console.WriteLine("To exit press f");
        char ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        try
        {
            while (ch != 'f')
            {
                switch (ch)
                {
                    case 'a': // Create a new milestone.
                        CreateMilestone();
                        break;
                    case 'b': // Read a milestone by ID.
                        Console.WriteLine("Enter a task ID");
                        int idMilestone = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
                        ReadMilestone(idMilestone);
                        break;
                    case 'c': // Update a milestone.
                        UpdateMilestone();
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("To add a task press a");
                Console.WriteLine("To read a task press b");
                Console.WriteLine("To update a milestone press c");
                Console.WriteLine("To exit press f");
                ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }

    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        Console.WriteLine("For a task press 1");
        Console.WriteLine("For an engineer press 2");
        Console.WriteLine("For milestone between tasks press 3");
        Console.WriteLine("To exit press 0");
        string? chooseBeforeParse = Console.ReadLine();
        int.TryParse(chooseBeforeParse, out int choose);
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
                        Milestones();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("For a task press 1");
                Console.WriteLine("For an engineer press 2");
                Console.WriteLine("For depency between tasks press 3");
                Console.WriteLine("To exit press 0");
                chooseBeforeParse = Console.ReadLine();
                int.TryParse(chooseBeforeParse, out choose);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
