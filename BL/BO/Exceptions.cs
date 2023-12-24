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
