using DalApi;



internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
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

    // The function update a engineer.
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
    public static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
        Console.WriteLine("For a task press 1");
        Console.WriteLine("For an engineer press 2");
        Console.WriteLine("For depency between tasks press 3");
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
                        //_product(cart);
                        break;
                    case 2:
                        Engineers();
                        break;
                    case 3:
                        //cart = _cart(cart);
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
