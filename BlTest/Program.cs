using BO;
using DalApi;
using DO;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// The function create a new task
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">exeption of the entered value is incorrect</exception>
    public static void CreateTask()
    {
        Console.WriteLine("Enter a description of the task");
        string description = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter an alias of the task");
        string alias = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        BO.Status status = BO.Status.Unscheduled;
        Console.WriteLine("To add a dependency to a task, press 1");
        Console.WriteLine("Exit Press 0");
        string? chooseString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(chooseString, out int choose);
        List<BO.TaskInList>? tasks = new List<BO.TaskInList>();
        while (choose != 0)
        {
            Console.WriteLine("Enter the ID of all tasks that this task depends on");
            string? stringId = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(stringId, out int idDependency);
            BO.Task task = s_bl.Task.Read(idDependency) ?? throw new BO.BlDoesNotExistException($"Task with ID={idDependency} not exists");
            BO.TaskInList newTaskInList = new BO.TaskInList()
            {
                Id = idDependency,
                Alias = task.Alias,
                Description = task.Description
            };
            tasks.Add(newTaskInList);
            Console.WriteLine("To add a dependency to a task, press 1");
            Console.WriteLine("Exit Press 0");
            chooseString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(chooseString, out choose);
        }
        Console.WriteLine("Enter required effort time to the tast");
        TimeSpan requiredEffort = TimeSpan.Parse(Console.ReadLine() ?? throw new DalInvalidEnteredValue("The entered value is incorrect"));
        Console.WriteLine("Enter product deliverables of the task");
        string? deliverables = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter remarks of the task");
        string? remarks = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        Console.WriteLine("Enter the id of the engineer");
        string? chooseEngineerIdBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(chooseEngineerIdBeforeParse, out int engineerId);
        DateTime createdAt = DateTime.Now;
        Console.WriteLine("Enter the level of the task:");
        Console.WriteLine("For Beginner press 0");
        Console.WriteLine("For AdvancedBeginner press 1");
        Console.WriteLine("For Competent press 2");
        Console.WriteLine("For Proficient press 3");
        Console.WriteLine("For Expert press 4");
        string? chooseComplexityLevelBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(chooseComplexityLevelBeforeParse, out int complexityLevel);
        BO.Task newTask = new BO.Task()
        {
            Id = 0,
            Alias = alias,
            Description = description,
            CreatedAtDate = createdAt,
            Status = status,
            Dependencies = tasks,
            RequiredEffortTime = requiredEffort,
            Milestone = null,
            ScheduledStartDate = null,
            StartDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = new BO.EngineerInTask()
            {
                Id = engineerId,
                Name = s_bl?.Engineer.Read(engineerId)?.Name!
            },
            ComplexityLevel = (BO.EngineerExperience)complexityLevel
        };
        Console.WriteLine(s_bl!.Task.Create(newTask));
    }

    /// <summary>
    ///  The function update a task.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">exception: The entered value is incorrect</exception>
    /// <exception cref="BO.BlDoesNotExistException">exception: The engineer not exists</exception>
    public static void UpdateTask()

    {
        Console.WriteLine("Enter the task's id");
        int id = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        if (s_bl.Task.Read(id) != null)
        {
            Console.WriteLine(s_bl.Task.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} not exists");
        }
        BO.Task updateTask = s_bl.Task.Read(id)!;
        Console.WriteLine("Enter a description of the task");
        string description = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (description == "" || description == null)
        {
            description = updateTask.Description;
        }
        Console.WriteLine("Enter an alias of the task");
        string alias = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (alias == "" || alias == null)
        {
            alias = updateTask.Alias;
        }
        Console.WriteLine("Enter status of stak:");
        Console.WriteLine("For Unscheduled press 0");
        Console.WriteLine("For Scheduled press 1");
        Console.WriteLine("For OnTrack press 2");
        Console.WriteLine("For InJeopardy press 3");
        Console.WriteLine("For Done press 4");
        string? statusBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        BO.Status.TryParse(statusBeforeParse, out BO.Status status);
        if (statusBeforeParse == "" || statusBeforeParse == null)
        {
            status = (BO.Status)updateTask.Status!;
        }
        Console.WriteLine("To add a dependency to a task, press 1");
        Console.WriteLine("Exit Press 0");
        string? choose1 = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(choose1, out int choose);
        List<BO.TaskInList>? tasks = new List<BO.TaskInList>();
        while (choose != 0)
        {
            Console.WriteLine("Enter the ID of all tasks that this task depends on");
            string? stringId = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(stringId, out int idDependency);
            BO.Task task = s_bl.Task.Read(idDependency) ?? throw new BO.BlDoesNotExistException($"Task with ID={idDependency} not exists");
            BO.TaskInList newTaskInList = new BO.TaskInList()
            {
                Id = idDependency,
                Alias = task.Alias,
                Description = task.Description
            };
            tasks.Add(newTaskInList);
            Console.WriteLine("To add a dependency to a task, press 1");
            Console.WriteLine("Exit Press 0");
            choose1 = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(choose1, out choose);
        }
        if (tasks.Count == 0)
        {
            tasks = updateTask.Dependencies;
        }
        Console.WriteLine("Enter task start date");
        string? startDate1 = Console.ReadLine();
        DateTime? startDate;
        if (startDate1 == "" || startDate1 == null) //If not update the start date.
        {
            startDate = updateTask.StartDate;
        }
        else
        {
            startDate = DateTime.Parse(startDate1);
        }
        Console.WriteLine("Enter task complete date");
        string? completeDate1 = Console.ReadLine();
        DateTime? completeDate;
        if (completeDate1 == "" || completeDate1 == null) //If not update the complete date.
        {
            completeDate = updateTask.CompleteDate;
        }
        else
        {
            completeDate = DateTime.Parse(completeDate1);
        }
        Console.WriteLine("Enter product deliverables of the task");
        string? deliverables = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (deliverables == "" || deliverables == null)
        {
            deliverables = updateTask.Deliverables;
        }
        Console.WriteLine("Enter remarks of the task");
        string? remarks = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (remarks == "" || remarks == null)
        {
            remarks = updateTask.Remarks;
        }
        Console.WriteLine("Enter the id of the engineer");
        string? engineerIdBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(engineerIdBeforeParse, out int engineerId);
        if (engineerIdBeforeParse == "" || engineerIdBeforeParse == null)
        {
            engineerId = updateTask.Engineer!.Id;
        }
        DateTime createdAt = DateTime.Now;
        Console.WriteLine("Enter the level of the task:");
        Console.WriteLine("For Beginner press 0");
        Console.WriteLine("For AdvancedBeginner press 1");
        Console.WriteLine("For Competent press 2");
        Console.WriteLine("For Proficient press 3");
        Console.WriteLine("For Expert press 4");
        string? complexityLevelBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(complexityLevelBeforeParse, out int complexityLevel);
        if (complexityLevelBeforeParse == "" || complexityLevelBeforeParse == null)
        {
            complexityLevel = (int)updateTask.ComplexityLevel;
        }
        BO.Task newTask = new BO.Task()
        {
            Id = updateTask.Id,
            Alias = alias,
            Description = description,
            CreatedAtDate = createdAt,
            Status = status,
            Dependencies = tasks,
            RequiredEffortTime = updateTask.RequiredEffortTime,
            Milestone = updateTask.Milestone,
            ScheduledStartDate = null,
            StartDate = startDate,
            DeadlineDate = null,
            CompleteDate = completeDate,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = new BO.EngineerInTask()
            {
                Id = engineerId,
                Name = s_bl?.Engineer.Read(engineerId)?.Name!
            },
            ComplexityLevel = (BO.EngineerExperience)complexityLevel
        };
        try
        {
            s_bl!.Task.Update(newTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// The function read all the task.
    /// </summary>
    /// <exception cref="BO.BlDataListIsEmpty">The list is empty. There is no data to read.</exception>
    public static void ReadAllTasks()
    {
        IEnumerable<BO.Task?> tasks = s_bl.Task.ReadAll() ?? throw new BO.BlDataListIsEmpty("There are no tasks.");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    /// <summary>
    ///  The function read a task by ID.
    /// </summary>
    /// <param name="idTask">The id task to read</param>
    /// <exception cref="BlDoesNotExistException">exception: engineer with this id does not exists</exception>
    public static void ReadTask(int idTask)
    {
        if (s_bl.Task.Read(idTask) == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={idTask} not exists");
        }
        else
        {
            Console.WriteLine(s_bl.Task.Read(idTask));
        }
    }

    /// <summary>
    /// The function delete a task.
    /// </summary>
    /// <param name="idTaskDelete">The id task to delete</param>
    public static void DeleteTask(int idTaskDelete)
    {
        try
        {
            s_bl.Task.Delete(idTaskDelete); // Input the new id of the task.
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// The function of the tasks.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">The entered value is incorrect</exception>
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
    ///The function create a new engineer.
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
        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = (BO.EngineerExperience)level,
            Cost = cost,
            Task = null
        };
        Console.WriteLine(s_bl.Engineer.Create(newEngineer));
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
        if (s_bl.Engineer.Read(id) != null)
        {
            Console.WriteLine(s_bl.Engineer.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} not exists");
        }
        BO.Engineer updateEngineer = s_bl.Engineer.Read(id)!;
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
     
        BO.Engineer newEngineer = new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Level = (BO.EngineerExperience)level,
            Cost = cost,
            Task = null
        };
        try
        {
            s_bl.Engineer.Update(newEngineer);
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
    /// <exception cref="BlDoesNotExistException">exception: engineer with this id does not exists</exception>
    public static void ReadEngineer(int idEngineer)
    {
        if (s_bl.Engineer.Read(idEngineer) == null)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={idEngineer} not exists");
        }
        else
        {
            Console.WriteLine(s_bl.Engineer.Read(idEngineer));
        }
    }

    /// <summary>
    /// The function read all the engineers.
    /// </summary>
    /// <exception cref="BO.BlDataListIsEmpty">The list is empty. There is no data to read.</exception>
    public static void ReadAllEngineers()
    {
        IEnumerable<BO.Engineer?> engineers = s_bl.Engineer.ReadAll() ?? throw new BO.BlDataListIsEmpty("There are no engineers.");
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
            s_bl.Engineer.Delete(idEngineerDelete); // Input the new id of the task.
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// The function of the engineer.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">The entered value is incorrect</exception>
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

            Console.WriteLine(ex);
        }
    }


    /// <summary>
    ///  The function read a milestone by ID.
    /// </summary>
    /// <param name="idMilestone">The id milestone to read</param>
    /// <exception cref="BO.BlDoesNotExistException">exception: milestone with this id does not exists</exception>
    public static void ReadMilestone(int idMilestone)
    {
        if (s_bl.Milestone.Read(idMilestone) == null)
        {
            throw new BO.BlDoesNotExistException($"Milestone with ID={idMilestone} not exists");
        }
        else
        {
            Console.WriteLine(s_bl.Milestone.Read(idMilestone));
        }
    }

    /// <summary>
    /// update a milstone.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue"></exception>
    public static void UpdateMilestone()
    {
        Console.WriteLine("Enter the task's id");
        int id = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        if (s_bl.Milestone.Read(id) != null)
        {
            Console.WriteLine(s_bl.Task.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} not exists");
        }
        BO.Milestone updateTask = s_bl.Milestone.Read(id)!;
        Console.WriteLine("Enter a description of the task");
        string description = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (description == "" || description == null)
        {
            description = updateTask.Description;
        }
        Console.WriteLine("Enter an alias of the task");
        string alias = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (alias == "" || alias == null)
        {
            alias = updateTask.Alias;
        }
        Console.WriteLine("Enter remarks of the task");
        string? remarks = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (remarks == "" || remarks == null)
        {
            remarks = updateTask.Remarks;
        }
        BO.Milestone newMilestone = new BO.Milestone()
        {
            Id = updateTask.Id,
            Description = description,
            Alias = alias,
            CreatedAtDate = updateTask.CreatedAtDate,
            Status = updateTask.Status,
            StartDate = updateTask.StartDate,
            ForecastDate = updateTask.ForecastDate,
            DeadlineDate = updateTask.DeadlineDate,
            CompleteDate = updateTask.CompleteDate,
            CompletionPercentage = updateTask.CompletionPercentage,
            Remarks = remarks,
            Dependencies = updateTask.Dependencies
        };
        s_bl!.Milestone.Update(newMilestone);
    }

    /// <summary>
    ///The function of the Milestones.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">The entered value is incorrect</exception>
    public static void Milestones()
    {
        Console.WriteLine("To read a milstone press a");
        Console.WriteLine("To update a milestone press b");
        //Console.WriteLine("To create a project schedule press c");
        Console.WriteLine("To exit press f");
        char ch = char.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        try
        {
            while (ch != 'f')
            {
                switch (ch)
                {
                    //case 'c': // Read a milestone by ID.
                    //    s_bl.Milestone.Create();
                    //    break;
                    case 'a': // Read a milestone by ID.
                        Console.WriteLine("Enter a milstone ID");
                        int idMilestone = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
                        ReadMilestone(idMilestone);
                        break;
                    case 'b': // Update a milestone.
                        UpdateMilestone();
                        break;
                    default:
                        Console.WriteLine("The letter entered is invalid");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("To read a milstone press a");
                Console.WriteLine("To update a milestone press b");
                //Console.WriteLine("To create a project schedule press c");
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
    /// Main function
    /// </summary>
    /// <exception cref="FormatException">wronginput</exception>
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the project start date (yyyy-MM-ddTHH:mm:ss):");
        string? startDateString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(startDateString, out DateTime startDate);
        Console.WriteLine("Enter the project end date (yyyy-MM-ddTHH:mm:ss):");
        string? endDateString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(endDateString, out DateTime endDate);
        if (startDateString != "" && endDateString != "")
        {
            Tools.SetProjectDates(startDate, endDate);
        }
        Console.WriteLine("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        if (ans == "Y")
        {
            
            DalTest.Initialization.Do();
        }
        Console.WriteLine("Add all engineers");
        try
        {
            Engineers();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        Console.WriteLine();
        Console.WriteLine("Add all tasks");
        try
        {
            Tasks();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        s_bl.Milestone.Create();
        Console.WriteLine();
        Console.WriteLine("For a task press 1");
        Console.WriteLine("For an engineer press 2");
        Console.WriteLine("For milestone between tasks press 3");
        Console.WriteLine("To exit press 0");
        string? chooseBeforeParseAll = Console.ReadLine();
        int.TryParse(chooseBeforeParseAll, out int ch);
        try
        {
            while (ch != 0)
            {
                switch (ch)
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
                Console.WriteLine("For milestone between tasks press 3");
                Console.WriteLine("To exit press 0");
                chooseBeforeParseAll = Console.ReadLine();
                int.TryParse(chooseBeforeParseAll, out ch);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }
}