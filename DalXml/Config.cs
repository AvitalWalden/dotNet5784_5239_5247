namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }

    internal static DateTime? startDateProject = XMLTools.LoadListFromXMLElement(@"..\xml\data-config.xml").ToDateTimeNullable("startDateProject");
   
    internal static DateTime? endDateProject = XMLTools.LoadListFromXMLElement(@"..\xml\data-config.xml").ToDateTimeNullable("endDateProject");

}