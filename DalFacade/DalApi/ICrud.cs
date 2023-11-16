namespace DalApi;

public interface ICrud<T> where T : class
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(Func<T, bool> filter); // stage 2
    T? Read(int id); //Reads entity object by its ID 
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); //Reads objects 
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}

