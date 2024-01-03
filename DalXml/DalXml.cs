using DalApi;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;

sealed internal class DalXml : IDal
{
    //public static IDal Instance { get; } = new DalXml();

    private static readonly Lazy<DalXml> lazyInstance = new Lazy<DalXml>(() => new DalXml());
    public static IDal Instance => lazyInstance.Value;

    //private static readonly object lockObject = new object();
    private DalXml() { }
    public ITask Task => new TaskImplementation();

    public IDependency Dependency =>  new DependencyImplementation();

    public IEngineer Engineer =>  new Engineerlementation();

    //public DateTime? startDateProject => Config.startDateProject;

    //public DateTime? endDateProject => Config.endDateProject;
    private DateTime? ParseDateTime(string dateTimeString)
    {
        if (DateTime.TryParse(dateTimeString, out DateTime result))
        {
            return result;
        }
        return null;
    }
    public DateTime? startDateProject
    {
        get
        {
            var value = XDocument.Load(@"..\xml\data-config.xml").Root?.Element("startProject")?.Value;
            return string.IsNullOrEmpty(value) ? null : DateTime.Parse(value);
        }
        set
        {
            var xDocument = XDocument.Load(@"..\xml\data-config.xml");
            xDocument.Root?.Element("startProject")?.SetValue(value?.ToString("yyyy-MM-ddTHH:mm:ss")!);
            xDocument.Save(@"..\xml\data-config.xml");
        }

    }

    public DateTime? endDateProject 
    {
        get
        {
            var value = XDocument.Load(@"..\xml\data-config.xml").Root?.Element("endDateProject")?.Value;
            return string.IsNullOrEmpty(value) ? null : DateTime.Parse(value);
        }
        set
        {
            var xDocument = XDocument.Load(@"..\xml\data-config.xml");
            xDocument.Root?.Element("endDateProject")?.SetValue(value?.ToString("yyyy-MM-ddTHH:mm:ss")!);
            xDocument.Save(@"..\xml\data-config.xml");
        }
    }

    public void Reset()
    {
        Task.Reset();
        Dependency.Reset();
        Engineer.Reset();
    }
}
