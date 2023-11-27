using static System.Runtime.InteropServices.JavaScript.JSType;

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

[Serializable]
public class DalInvalidEnteredValue: Exception // Exception of Invalid entered value.
{
    public DalInvalidEnteredValue(string? message) : base(message) { }
}

[Serializable]
public class DalDataListIsEmpty: Exception // Exception of The data list is empty.
{
    public DalDataListIsEmpty(string? message) : base(message) { }
}