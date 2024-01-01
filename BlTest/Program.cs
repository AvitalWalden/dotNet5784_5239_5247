


using DalApi;

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
        //// Console.WriteLine("Enter task Created task date");
        Console.WriteLine("Enter status of stak:");
        Console.WriteLine("For Unscheduled press 0");
        Console.WriteLine("For Scheduled press 1");
        Console.WriteLine("For OnTrack press 2");
        Console.WriteLine("For InJeopardy press 3");
        Console.WriteLine("For Done press 4");
        string? chooseStatusBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        BO.Status.TryParse(chooseStatusBeforeParse, out BO.Status status);
        Console.WriteLine("To add a dependency to a task, press 1");
        Console.WriteLine("Exit Press 0");
        string? chooseString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(chooseString, out int choose);
        List<BO.TaskInList>? tasks = new List<BO.TaskInList>();
        while (choose != 0)
        {
            Console.WriteLine("Enter id of the task that dependency on this task");
            string? stringId = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(stringId, out int idDependency);
            Console.WriteLine("Enter alias of the task that dependency on this task");
            string aliasOfidDependency = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter description of the task that dependency on this task");
            string descriptionOfidDependency = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            BO.TaskInList newTaskInList = new BO.TaskInList()
            {
                Id = idDependency,
                Alias = aliasOfidDependency,
                Description = descriptionOfidDependency
            };
            tasks.Add(newTaskInList);
            Console.WriteLine("To add a dependency to a task, press 1");
            Console.WriteLine("Exit Press 0");
            chooseString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(chooseString, out choose);
        }
        Console.WriteLine("Enter id of milestone in task");
        string? chooseIdMilstoneBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(chooseIdMilstoneBeforeParse, out int idMilstone);
        Console.WriteLine("Enter scheduled startDate date");
        string? chooseScheduledStartDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(chooseScheduledStartDateBeforeParse, out DateTime scheduledStartDate);
        Console.WriteLine("Enter task start date");
        string? choosestartDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(choosestartDateBeforeParse, out DateTime startDate);
        Console.WriteLine("Enter task forecast date");
        string? chooseforecastDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(chooseforecastDateBeforeParse, out DateTime forecastDate);
        Console.WriteLine("Enter task deadline date");
        string? chooseDeadlineDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(chooseDeadlineDateBeforeParse, out DateTime deadlineDate);
        Console.WriteLine("Enter task complete date");
        string? choosecompleteDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(choosecompleteDateBeforeParse, out DateTime completeDate);
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
            Milestone = new BO.MilestoneInTask()
            {
                Id = idMilstone,
                Alias = s_bl.Task.Read(idMilstone)?.Alias!
            },
            BaselineStartDate = scheduledStartDate,
            StartDate = startDate,
            ForecastDate = forecastDate,
            DeadlineDate = deadlineDate,
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
        if (s_bl?.Task.Read(id) != null)
        {
            Console.WriteLine(s_bl?.Task.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} not exists");
        }
        BO.Task updateTask = s_bl?.Task.Read(id)!;
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
        //// Console.WriteLine("Enter task Created task date");
        Console.WriteLine("Enter status of stak:");
        Console.WriteLine("For Unscheduled press 0");
        Console.WriteLine("For Scheduled press 1");
        Console.WriteLine("For OnTrack press 2");
        Console.WriteLine("For InJeopardy press 3");
        Console.WriteLine("For Done press 4");
        string? statusBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        BO.Status.TryParse(statusBeforeParse, out BO.Status status);
        if(statusBeforeParse == "" || statusBeforeParse == null)
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
            Console.WriteLine("Enter id of the task that dependency on this task");
            string? stringId = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(stringId, out int idDependency);
            Console.WriteLine("Enter alias of the task that dependency on this task");
            string aliasOfidDependency = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            Console.WriteLine("Enter description of the task that dependency on this task");
            string descriptionOfidDependency = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            BO.TaskInList newTaskInList = new BO.TaskInList()
            {
                Id = idDependency,
                Alias = aliasOfidDependency,
                Description = descriptionOfidDependency
            };
            tasks.Add(newTaskInList);
            Console.WriteLine("To add a dependency to a task, press 1");
            Console.WriteLine("Exit Press 0");
            choose1 = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
            int.TryParse(choose1, out choose);
        }
        if ( choose == 0 ) {
            tasks = updateTask.Dependencies;
        }
        Console.WriteLine("Enter id of milestone in task");
        string? idMilstoneBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        int.TryParse(idMilstoneBeforeParse, out int idMilstone);
        if (idMilstoneBeforeParse == "" || idMilstoneBeforeParse == null)
        {
            idMilstone = updateTask.Milestone!.Id;
        }
        Console.WriteLine("Enter baseline startDate date");
        string? baselineDate1 = Console.ReadLine();
        DateTime? baselinedDate;
        if (baselineDate1 == "" || baselineDate1 == null) //If not update the complete date.
        {
            baselinedDate = updateTask.BaselineStartDate;
        }
        else
        {
            baselinedDate = DateTime.Parse(baselineDate1);
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
        Console.WriteLine("Enter task forecast date");
        string? forecastDate1 = Console.ReadLine();
        DateTime? forecastDate;
        if (forecastDate1 == "" || forecastDate1 == null) //If not update the complete date.
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
            deadlineDate = updateTask.DeadlineDate;
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
        if(engineerIdBeforeParse == "" || engineerIdBeforeParse == null)
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
            Id = 0,
            Alias = alias,
            Description = description,
            CreatedAtDate = createdAt,
            Status = status,
            Dependencies = tasks,
            Milestone = new BO.MilestoneInTask()
            {
                Id = idMilstone,
                Alias = s_bl?.Task.Read(idMilstone)?.Alias!
            },
            BaselineStartDate = baselinedDate,
            StartDate = startDate,
            ForecastDate = forecastDate,
            DeadlineDate = deadlineDate,
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
        IEnumerable<BO.Task?> tasks = s_bl?.Task.ReadAll() ?? throw new BO.BlDataListIsEmpty("There are no tasks.");
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
        if (s_bl?.Task.Read(idTask) == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={idTask} not exists");
        }
        else
        {
            Console.WriteLine(s_bl?.Task.Read(idTask));
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
            s_bl?.Task.Delete(idTaskDelete); // Input the new id of the task.
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
    /// <exception cref="BlDoesNotExistException">exception: engineer with this id does not exists</exception>
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

    public static void CreateMilestone()
    {
        //Console.WriteLine("Enter a description of the task");
        //string description = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //Console.WriteLine("Enter an alias of the task");
        //string alias = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        ////// Console.WriteLine("Enter task Created task date");
        //Console.WriteLine("Enter status of stak");
        //string? chooseStatusBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //BO.Status.TryParse(chooseStatusBeforeParse, out BO.Status status);
        //Console.WriteLine("Enter task start date");
        //string? choosestartDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //DateTime.TryParse(choosestartDateBeforeParse, out DateTime startDate);
        //Console.WriteLine("Enter task forecast date");
        //string? chooseforecastDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //DateTime.TryParse(chooseforecastDateBeforeParse, out DateTime forecastDate); Console.WriteLine("Enter task deadline date");
        //Console.WriteLine("Enter task deadline date");
        //string? chooseDeadlineDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //DateTime.TryParse(chooseDeadlineDateBeforeParse, out DateTime deadlineDate); Console.WriteLine("Enter task deadline date");
        //Console.WriteLine("Enter task complete date");
        //string? choosecompleteDateBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //DateTime.TryParse(choosecompleteDateBeforeParse, out DateTime completeDate); Console.WriteLine("Enter task deadline date");
        //string? chooseComplexityLevelBeforeParse = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        //int.TryParse(chooseComplexityLevelBeforeParse, out int complexityLevel);
        //Console.WriteLine("Enter remarks of the task");
        //string? remarks = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
    }
    /// <summary>
    ///  The function read a milestone by ID.
    /// </summary>
    /// <param name="idMilestone">The id milestone to read</param>
    /// <exception cref="BO.BlDoesNotExistException">exception: milestone with this id does not exists</exception>
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

    /// <summary>
    /// update a milstone.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue"></exception>
    public static void UpdateMilestone()
    {
        Console.WriteLine("Enter the task's id");
        int id = int.Parse(Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect"));
        if (s_bl?.Task.Read(id) != null)
        {
            Console.WriteLine(s_bl?.Task.Read(id));
        }
        else
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} not exists");
        }
        BO.Task updateTask = s_bl?.Task.Read(id)!;
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
            Id = 0,
            Description = description,
            Alias = alias,
            CreatedAtDate = DateTime.Now,
            Status = null,
            StartDate = null,
            ForecastDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            CompletionPercentage = null,
            Remarks = remarks,
            Dependencies = null
        };
        s_bl!.Milestone.Update(newMilestone);
    }

    /// <summary>
    ///The function of the Milestones.
    /// </summary>
    /// <exception cref="BO.BlInvalidEnteredValue">The entered value is incorrect</exception>
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
        {
            Factory.Get.Reset();
            DalTest.Initialization.Do();
        }
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
        Console.WriteLine("Enter the project start date (yyyy-MM-ddTHH:mm:ss):");
        string? startDateString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(startDateString, out DateTime startDate);
        DalApi.Factory.Get.startDateProject = startDate; 
        Console.WriteLine("Enter the project end date (yyyy-MM-ddTHH:mm:ss):");
        string? endDateString = Console.ReadLine() ?? throw new BO.BlInvalidEnteredValue("The entered value is incorrect");
        DateTime.TryParse(endDateString, out DateTime endDate);
        DalApi.Factory.Get.endDateProject = endDate;
    }
}
