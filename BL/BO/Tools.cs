using System.Reflection;

namespace BO;

internal  static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        string result = string.Join(", ", properties.Select(property =>
        {
            object? value = property.GetValue(obj);
            string? valueString = (value != null) ? value.ToString() : "null";
            return $"{property.Name}: {valueString}";
        }));

        return result;
    }
}
