using BlApi;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creating an assignment task for the project
    /// </summary>
    /// <param name="boTask">the BO task</param>
    /// <returns>ID of the new task</returns>
    /// <exception cref="BO.BlInvalidValue">A error that the value is invvalid value</exception>
    /// <exception cref="BO.EngineerIsAlreadyBusy">A error that the engineer is busy</exception>
    /// <exception cref="BO.BlAlreadyExistsException">error</exception>
    public int Create(BO.Task boTask)
    {
        if (boTask.Id < 0)
        {
            throw new BO.BlInvalidValue("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Alias))
        {
            throw new BO.BlInvalidValue("Task Alias cannot be empty or null");
        }
        var task = _dal.Task.Read(task => task.EngineerId == boTask.Engineer?.Id && BO.Tools.CalculateStatusOfTask(task) != BO.Status.Done);
        if (task != null && task.Id!= boTask.Id)
        {
            throw new BO.BlEngineerIsAlreadyBusy("Engineer is already busy");
        }
        if (boTask.Engineer != null && _dal.Engineer.Read(boTask.Engineer!.Id) == null)
        {
            throw new BO.BlEngineerDoesNotExit("There is no engineer with such an ID");
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Description,
            boTask.Alias,
            boTask.CreatedAtDate,
            boTask.RequiredEffortTime,
            false,
            boTask.StartDate,
            boTask.ScheduledStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id,
            (DO.EngineerExperience)boTask.ComplexityLevel
        );
       
        try
        {
            int idTask = _dal.Task.Create(doTask);
            if (boTask.Dependencies != null)
            {
                foreach (BO.TaskInList doDependency in boTask.Dependencies)
                {
                    DO.Dependency doDepend = new DO.Dependency(0, idTask, doDependency.Id);
                    int idDependency = _dal.Dependency.Create(doDepend);
                }
            }
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Task details request
    /// </summary>
    /// <param name="id">the id of the task</param>
    /// <returns>a task</returns>
    /// <exception cref="BO.BlDoesNotExistException">errors that the task with this id does Not exist</exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        DO.Engineer? eng = _dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId);
        BO.EngineerInTask? engineer = null;
        if (eng != null)
        {
            engineer = new BO.EngineerInTask()
            {
                Id = eng.Id,
                Name = eng.Name
            };
        }

        List<BO.TaskInList>? tasksList = null;
        BO.MilestoneInTask? milestone = null;

        DO.Dependency? checkMilestone = _dal.Dependency.Read(dependency => dependency.DependsOnTask == doTask.Id);
        if (checkMilestone != null)
        {
            int milestoneId = checkMilestone.DependentTask;
            DO.Task? milestoneAsATask = _dal.Task.Read(task => task.Id == milestoneId && task.IsMilestone);
            if (milestoneAsATask != null)
            {
                string aliasOfMilestone = milestoneAsATask.Alias;
                milestone = new BO.MilestoneInTask()
                {
                    Id = milestoneId,
                    Alias = aliasOfMilestone
                };
            }
            else
            {
                tasksList = BO.Tools.CalculateTaskInList(id);
            }
        }
        else
        {
            tasksList = BO.Tools.CalculateTaskInList(id);
        }
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = BO.Tools.CalculateStatusOfTask(doTask),
            Dependencies = tasksList,
            RequiredEffortTime = doTask.RequiredEffort,
            Milestone = milestone,
            ScheduledStartDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = engineer,
            ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
        };
    }

    /// <summary>
    /// Request details of all tasks
    /// </summary>
    /// <param name="filter">A function for mapping the tasks</param>
    /// <returns>Returns all tasks</returns>
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        IEnumerable<BO.Task?> readAllTask = _dal.Task.ReadAll().Select(doTask =>
        {
            if (doTask == null)
            {
                return null;
            }
            DO.Engineer? eng = _dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId);
            BO.EngineerInTask? engineer = null;
            if (eng != null)
            {
                engineer = new BO.EngineerInTask()
                {
                    Id = eng.Id,
                    Name = eng.Name
                };
            }
            List<BO.TaskInList>? tasksList = null;
            BO.MilestoneInTask? milestone = null;

            DO.Dependency? checkMilestone = _dal.Dependency.Read(dependency => dependency.DependsOnTask == doTask.Id);
            if (checkMilestone != null)
            {
                int milestoneId = checkMilestone.DependentTask;
                DO.Task? milestoneAsATask = _dal.Task.Read(task => task.Id == milestoneId && task.IsMilestone);
                if (milestoneAsATask != null)
                {
                    string aliasOfMilestone = milestoneAsATask.Alias;
                    milestone = new BO.MilestoneInTask()
                    {
                        Id = milestoneId,
                        Alias = aliasOfMilestone
                    };
                }
                else
                {
                    tasksList = BO.Tools.CalculateTaskInList(doTask.Id);
                }
            }
            else
            {
                tasksList = BO.Tools.CalculateTaskInList(doTask.Id);
            }
            return new BO.Task
            {
                Id = doTask.Id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = BO.Tools.CalculateStatusOfTask(doTask),
                Dependencies = tasksList,
                RequiredEffortTime = doTask.RequiredEffort,
                Milestone = milestone,
                ScheduledStartDate = doTask.ScheduledDate,
                StartDate = doTask.StartDate,
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer = engineer,
                ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
            };


        }).Where(task => task != null).ToList(); // We will use WHERE to filter and drop the tasks that are NULL

        if (filter != null)
        {
            IEnumerable<BO.Task> readAllTaskFilter = from item in readAllTask
                                                     where filter(item)
                                                     select item;
            return readAllTaskFilter;
        }
        return readAllTask;
    }

    /// <summary>
    /// task to update
    /// </summary>
    /// <param name="boTask">the task to update</param>
    /// <exception cref="BO.BlInvalidValue">A error that the value is invvalid value</exception>
    /// <exception cref="BO.EngineerIsAlreadyBusy">A error that the engineer is busy</exception>
    /// <exception cref="BO.BlAlreadyExistsException">error</exception>
    public void Update(BO.Task boTask)
    {
        if (boTask.Engineer != null && _dal.Engineer.Read(boTask.Engineer!.Id) == null)
        {
            throw new BO.BlEngineerDoesNotExit("There is no engineer with such an ID");
        }
        if (boTask.Id < 0)
        {
            throw new BO.BlInvalidValue("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Alias))
        {
            throw new BO.BlInvalidValue("Task alias cannot be empty or null");
        }
        if (string.IsNullOrWhiteSpace(boTask.Description))
        {
            throw new BO.BlInvalidValue("Task Description cannot be empty or null");
        }
        TimeSpan? requiredEffort = null;
        if (boTask.StartDate != null && boTask.CompleteDate != null)
        {
            requiredEffort = (TimeSpan)(boTask.StartDate! - boTask.CompleteDate!);
        }
        var task = _dal.Task.Read(task => task.EngineerId == boTask.Engineer?.Id && BO.Tools.CalculateStatusOfTask(task) != BO.Status.Done);
        if (task != null && task.Id != boTask.Id)
        {
            throw new BO.BlEngineerIsAlreadyBusy("Engineer is already busy");
        }
        if (boTask.Milestone == null)
        {
            foreach (var item in _dal.Dependency.ReadAll(d => d.DependentTask == boTask.Id).Where(d=>d!=null))
            {
                _dal.Dependency.Delete(item!.Id);
            }
            if (boTask.Dependencies != null)
            {
                foreach (BO.TaskInList doDependency in boTask.Dependencies)
                {
                    DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                    int idDependency = _dal.Dependency.Create(doDepend);
                }
            }
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Description,
            boTask.Alias,
            boTask.CreatedAtDate,
            boTask.RequiredEffortTime,
            false,
            boTask.StartDate,
            boTask.ScheduledStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id,
            (DO.EngineerExperience)boTask.ComplexityLevel
        );

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
    
    /// <summary>
    /// Delete the task
    /// </summary>
    /// <param name="id">ID of the task ti delete</param>
    /// <exception cref="BO.BlDoesNotExistException">a error that the this id does not exist</exception>
    public void Delete(int id)
    {
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message, ex);
        }
    }
}