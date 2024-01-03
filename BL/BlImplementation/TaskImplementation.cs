using BlApi;
using BO;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {

        if (boTask.Id < 0)
        {
            throw new BO.BlInvalidValue("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Alias))
        {
            throw new BO.BlInvalidValue("Task description cannot be empty or null");
        }
        TimeSpan? requiredEffort = null;
        if (boTask.StartDate != null || boTask.CompleteDate != null)
        {
            requiredEffort = (TimeSpan)(boTask.StartDate! - boTask.CompleteDate!);
        }
        var task = _dal.Task.Read(task => task.EngineerId == boTask.Engineer?.Id && Tools.CalculateStatusOfTask(task) != BO.Status.Done);
        if (task != null)
        {
            throw new BO.EngineerIsAlreadyBusy("Engineer is already busy");
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Description,
            boTask.Alias,
            boTask.CreatedAtDate,
            requiredEffort,
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
        if (boTask.Dependencies != null)
        {
            foreach (BO.TaskInList doDependency in boTask.Dependencies)
            {
                DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                int idDependency = _dal.Dependency.Create(doDepend);
            }
        }
        try
        {
            int idTask = _dal.Task.Create(doTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
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
                tasksList = Tools.CalculateTaskInList(id);
            }
        }
        else
        {
            tasksList = Tools.CalculateTaskInList(id);
        }
        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = Tools.CalculateStatusOfTask(doTask),
            Dependencies = tasksList,
            Milestone = milestone,
            ScheduledStartDate = doTask.ScheduledDate,
            //ScheduledStartDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            ForecastDate = doTask.StartDate + doTask.RequiredEffort,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = engineer,
            ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
        };
    }
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
                    tasksList = Tools.CalculateTaskInList(doTask.Id);
                }
            }
            else
            {
                tasksList = Tools.CalculateTaskInList(doTask.Id);
            }
            return new BO.Task
            {
                Id = doTask.Id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = Tools.CalculateStatusOfTask(doTask),
                Dependencies = tasksList,
                Milestone = milestone,
                ScheduledStartDate = doTask.ScheduledDate,
                StartDate = doTask.StartDate,
                //ScheduledStartDate = doTask.ScheduledDate,
                ForecastDate = doTask.StartDate + doTask.RequiredEffort,
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
    public void Update(BO.Task boTask)
    {
        if (boTask.Id < 0)
        {
            throw new Exception("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Description))
        {
            throw new Exception("Task description cannot be empty or null");
        }
        TimeSpan? requiredEffort = null;
        if (boTask.StartDate != null || boTask.CompleteDate != null)
        {
            requiredEffort = (TimeSpan)(boTask.StartDate! - boTask.CompleteDate!);
        }
        var task = _dal.Task.Read(task => task.EngineerId == boTask.Engineer?.Id && Tools.CalculateStatusOfTask(task) != BO.Status.Done);
        if (task != null && task.Id != boTask.Id)
        {
            throw new BO.EngineerIsAlreadyBusy("Engineer is already busy");
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            requiredEffort,
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

        if (boTask.Milestone != null)
        {
            if (boTask.Dependencies != null)
            {
                foreach (BO.TaskInList doDependency in boTask.Dependencies)
                {
                    DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                    int idDependency = _dal.Dependency.Create(doDepend);
                }
            }
        }

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
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
