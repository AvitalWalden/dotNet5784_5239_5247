namespace BO;
//namespace DO;
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

[Serializable]
public class BlPlanningOfProjectTimesException : Exception
{
    public BlPlanningOfProjectTimesException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

}

[Serializable]
public class BlInvalidValue : Exception
{
    public BlInvalidValue(string? message) : base(message) { }
}

[Serializable]
public class BlInvalidEnteredValue : Exception
{
    public BlInvalidEnteredValue(string? message) : base(message) { }
}


[Serializable]
public class BlDataListIsEmpty : Exception // Exception of The data list is empty.
{
    public BlDataListIsEmpty(string? message) : base(message) { }
}

[Serializable]
public class FailedToReadMilestone : Exception // Exception of failed to read Milestone
{
    public FailedToReadMilestone(string? message) : base(message) { }
}


[Serializable]
public class BlFailedToCreateMilestone : Exception // Exception of failed to read Milestone
{
    public BlFailedToCreateMilestone(string? message) : base(message) { }
    public BlFailedToCreateMilestone(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BlEngineerIsAlreadyBusy : Exception 
{
    public BlEngineerIsAlreadyBusy(string? message) : base(message) { }
    public BlEngineerIsAlreadyBusy(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BlEngineerDoesNotExit : Exception
{
    public BlEngineerDoesNotExit(string? message) : base(message) { }
    public BlEngineerDoesNotExit(string message, Exception innerException) : base(message, innerException) { }
}

