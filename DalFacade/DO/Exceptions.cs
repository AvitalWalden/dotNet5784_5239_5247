namespace DO;

[Serializable] 
public class DalDoesNotExistException : Exception // Exception of object that does not exist.
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class DalAlreadyExistsException : Exception // Exception of object that already exist.
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalDeletionImpossible : Exception // Exception of object that can't not be deleted.
{
    public DalDeletionImpossible(string? message) : base(message) { }
}