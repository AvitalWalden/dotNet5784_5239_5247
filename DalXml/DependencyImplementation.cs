namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        const string dependenciesFile = @"..\xml\dependencies.xml";
        //XElement dependenciesElement = XElement.Load(dependenciesFile); // למה אין כאן .ROOT והאם צריך ?
        XElement? allDependencies = XDocument.Load(dependenciesFile).Root;

        if (allDependencies!.Elements("Dependency").Any(dependency => (int)dependency.Element("Id")! == id)) //האם אפשר לשים !
            throw new DalAlreadyExistsException($"Dependency with ID={id} already exists");

        XElement newDependencyElement = new XElement("Dependency",
            new XElement("Id", id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)
            );
        allDependencies.Add(newDependencyElement);
        allDependencies.Save(dependenciesFile);
        return id;
    }

    public void Delete(int id)
    {

        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;
        XElement? deleteDependency = allDependencies?.Elements().ToList().Find(dependency => Convert.ToInt32(dependency?.Element("Id")?.Value) == id);
        if (deleteDependency == null)
        {
            throw new DalDoesNotExistException($"dependency with ID={id} not exists");
        }
        deleteDependency!.Remove();
        allDependencies?.Save(@"..\xml\dependencies.xml");
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => filter(new Dependency(
                        (int)dependency.Element("Id")!,
                        (int)dependency.Element("DependentTask")!,
                        (int)dependency.Element("DependsOnTask")!
                    )));

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency(
                (int)dependencyElement.Element("Id")!,
                (int)dependencyElement.Element("DependentTask")!,
                (int)dependencyElement.Element("DependsOnTask")!
            );
            return dependency;
        }
        return null;
    }

    public Dependency? Read(int id)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => (int)dependency?.Element("Id")! == id); //האם אפשר לשים !

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency((int)dependencyElement.Element("Id")!, (int)dependencyElement.Element("DependentTask")!, (int)dependencyElement.Element("DependsOnTask")!);
            return dependency;
        }
        return null;
    }

    //public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    //{
    //    XElement? dependenciesElement = XMLTools.LoadListFromXMLElement("ArrayOfDependency");
    //    IEnumerable<Dependency> dependencies = dependenciesElement
    //        .Elements("Dependency")
    //        .Select(e => new Dependency(
    //            Id: (int)e.Element("id")!,
    //            DependentTask: (int)e.Element("dependentTask")!,
    //            DependsOnTask: (int)e.Element("dependsOnTask")!
    //        ));

    //    if (filter != null)
    //    {
    //        dependencies = dependencies.Where(filter);
    //    }

    //    return dependencies.ToList(); // Convert to List before returning

    //    //if (filter != null)
    //    //{
    //    //    return from item in DataSource.Dependencies
    //    //           where filter(item)
    //    //           select item;
    //    //}
    //    //// If no filter is provided, return all dependencies
    //    //return from item in DataSource.Dependencies
    //    //       select item;
    //}

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement("ArrayOfDependency");
        IEnumerable<Dependency> dependencies = dependenciesElement
            .Elements("Dependency")
            .Select(dependency => new Dependency
            {
                Id = (int)dependency.Element("id")!,
                DependentTask = (int)dependency.Element("dependentTask")!,
                DependsOnTask = (int)dependency.Element("dependsOnTask")!
            });

        if (filter != null)
        {
            dependencies = dependencies.Where(filter);
        }

        return dependencies;
    }

    /*
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Lesson7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"..\..\..\Myconfig.xml";
            const string filename2 = @"..\..\..\String.xml";
            const string filename3 = @"..\..\..\String.json";
            XDocument doc= XDocument.Load(filename);
            var c1=doc.Root?.Elements("config")?.
                Where(c => c.Attribute("name")?.Value == "Group1")?
                .FirstOrDefault()?
                .Descendants("bgcolor").FirstOrDefault()?.Value ;


            var c2=doc.Root?.Descendants("bgcolor")?.
                Where(c=>c.Ancestors("config")?.FirstOrDefault()?.Attribute("name")?.Value=="group1")?
                .FirstOrDefault()?.Value ;

            // הוספה 
            XElement e = new XElement("config", new XAttribute("name","group3"));
            XElement el = new XElement("bgcolor");
            el.Value = "brown";
            e.Add(el);
            doc.Root?.Add(e);
            doc.Save(filename);

            // הנתונים
            List<string> list = new List<string>() { "sara", "avigail", "ayal", "avital" };
            // אוביקט שיודע לשטוח אוביקט לסטרינג במבנה של אקס אמ אל
            XmlSerializer ser = new XmlSerializer(typeof(List<string>));
            // מצביע לקובץ 
            StreamWriter w = new StreamWriter(filename2);
            // הפעולה עצמה
            ser.Serialize(w, list);
            // שחרור המצביע
            w.Close();

            StreamReader reader = new StreamReader(filename2);
            List<string> lst = (List<string>?)ser.Deserialize(reader) ?? throw new Exception(); ;
            reader.Close();


            string str=JsonSerializer.Serialize<List<string>>(lst);
            StreamWriter wjson = new StreamWriter(filename3);
            wjson.Write(str);
            wjson.Close();

            // הגדרת מצביע לקריאה מהקובץ
            StreamReader rjson = new StreamReader(filename3);
            // קריאת הנתונים
            string s = rjson.ReadToEnd();
            // המרה לאוביקט
            List<string> strs=  JsonSerializer.Deserialize<List<string>>(s);
            rjson.Close();
           
            //doc.Root?.Descendants("config")
            //    .Where(c=>c.Attribute("name")?.Value == "group3")
            //    .Remove();
            //doc.Save(filename);

            Console.WriteLine("Hello, World!");


        }
    }
}*/

    public void Update(Dependency item)
    {
        int id = item.Id;
        // call to XML file
        const string dependenciesFile = @"..\xml\dependencies.xml";
        XElement? allDependencies = XDocument.Load(dependenciesFile).Root;
        // Check if there is an engineer with the same ID
        XElement? dependencyToUpdate = allDependencies!.Descendants("Dependency").FirstOrDefault(dependency => (int)dependency.Element("Id")! == id);
        if (dependencyToUpdate != null)
        {
            // Update the task in the XML file
            dependencyToUpdate.ReplaceWith(
                new XElement("Dependency",
                new XElement("Id", id),
                new XElement("DependentTask", item.DependentTask),
                new XElement("DependsOnTask", item.DependsOnTask)
                )
            );
            // Save to XML file again
            allDependencies.Save(dependenciesFile);
        }
        else
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        }
    }
}

